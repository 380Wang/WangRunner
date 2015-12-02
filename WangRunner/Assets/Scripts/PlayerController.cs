using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float boostForce = 6.0f;
    public float boostDuration = 3.0f;
    public float diveKickForce = 6.0f;
    public float diveKickDuration = 3.0f;
    public float movementSpeed = 3.0f;
    public float jumpForce = 200.0f;
    public float jetpackForce = 100.0f;
    public float glideFallFactor = 1.0f;
    public float uppercutForce = 12.0f;
    public float uppercutDuration = 3.0f;

    public Transform groundCheckTransformLeft;
    public Transform groundCheckTransformRight;
    public LayerMask groundCheckLayerMask;
    public Text currentScore;
    public Text debuggerOutput;
    public JumpAbility CurrentJump = JumpAbility.Jump;
    public ActionAbility CurrentAction = ActionAbility.Slide;

    private bool grounded = false;
    private bool lastGrounded = false;
    private bool isFirstTouch = true;
    private bool isLeftPressed = false;
    private bool isRightPressed = false;
    private bool isBoosting = false;
    private bool isDiveKicking = false;
    private bool isSingleJumping = false;
    private bool isJumping = false;
    private bool isStabilizing = false;
    private bool isSliding = false;
    private bool isUppercutting = false;
    private Rigidbody2D player;
    private float clickDelay = 0;
    private float doubleJumpCooldown = 0;
    private float attackCooldown = 0;
    private float boostCooldown = 0;
    private float diveKickCooldown = 0;
    private bool boostWasted = false;   // Used by Boost and Uppercut
    private bool diveKickWasted = false;
    private bool actionHappening = false;
    private float dropDistance;
    private bool slideFix = true;   // Don't worry about it...

    private GameObject killzone;
    private bool _isJetpackActive = false;
    public bool isJetpackActive
    {
        get { return _isJetpackActive; }
    }

    //===== DEBUG =====//
    private bool DEBUG = false;
    private bool DEBUG_MOBILE = false;
    //=================//

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        killzone = GameObject.Find("Killzone");
    }

    // Update is called once per frame
    void Update()
    {
        // Register touches
        if (Application.platform == RuntimePlatform.Android)
        {
            int tCount = Input.touchCount;
            if (tCount > 0)
            {
                //if (DEBUG_MOBILE) debuggerOutput.text = "Touchcount: " + tCount;
                foreach (var touch in Input.touches)
                {
                    if (TouchedGameplay(touch))
                    {
                        if (TouchedRightSide(touch))
                        {
                            RegisterTouch(RIGHT);
                        }
                        else
                        {
                            RegisterTouch(LEFT);
                        }
                    }
                }
            }
            else
            {
                //if (DEBUG_MOBILE) debuggerOutput.text = "tCount <= 0";
            }
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //== FOR DESKTOP: CONTROLS ==//
            if (Input.GetKey(KeyCode.RightArrow))
            {
                RegisterTouch(RIGHT);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                RegisterTouch(LEFT);
            }
            //=============================//
        }
        //Debug.Log ("Anchored position: " + (0.0f - titleBar.sizeDelta.y));
    }

    void FixedUpdate()
    {

        //Run();
        
        // Click delay hack to prevent accidental double jumps
        clickDelay -= Time.fixedDeltaTime;
        if (clickDelay < 0) clickDelay = 0;

        // Cooldowns and shit
        if (CurrentJump == JumpAbility.DoubleJump) doubleJumpCooldown -= Time.fixedDeltaTime;
        if (doubleJumpCooldown < 0) doubleJumpCooldown = 0;

        if (CurrentAction == ActionAbility.Attack) attackCooldown -= Time.fixedDeltaTime;
        if (attackCooldown < 0) attackCooldown = 0;

        // Recurring checks
        TrackTouch();
        UpdateAbilityStatus();
        UpdateGroundedStatus();
    }

    /**
    * The heart and core of most abilities
    **/
    void UpdateAbilityStatus()
    {
        //if (player.velocity.x < movementSpeed) Run();

        if (CurrentJump == JumpAbility.DoubleJump && CurrentAction != ActionAbility.Uppercut && boostWasted)
            boostWasted = false;

        if (CurrentAction == ActionAbility.Boost && boostCooldown > 0)
        {
            boostWasted = true;
            Vector2 boostVelo = player.velocity;
            boostVelo.x += 0.1f * boostForce;
            player.velocity = boostVelo;
            boostCooldown -= Time.fixedDeltaTime;
        }
        else if(CurrentAction == ActionAbility.Boost)
        {
            Reset();
        }

        if(CurrentJump == JumpAbility.DiveKick && diveKickCooldown > 0)
        {
            diveKickWasted = true;
            player.velocity = new Vector2(diveKickForce, -1.0f * diveKickForce);
            player.rotation = 45;
            diveKickCooldown -= Time.fixedDeltaTime;
        }
        else if(CurrentJump == JumpAbility.DiveKick && !isLeftPressed)
        {
            Reset();
        }

        if(CurrentJump == JumpAbility.Jetpack)
        {
            _isJetpackActive = isRightPressed ? true : false;
        }

        if(CurrentAction == ActionAbility.Slide && !isLeftPressed && !slideFix)
        {
            // Player was sliding, bring him back up by size/2 to prevent dropping glitch
            Vector2 currPos = player.position;
            currPos.y += 0.5f * player.transform.localScale.x;
            player.position = currPos;
            slideFix = true;
        }

        if(CurrentAction == ActionAbility.Uppercut && boostCooldown > 0)
        {
            boostWasted = true;
            player.velocity = new Vector2(uppercutForce, uppercutForce);
            player.rotation = -45;
            boostCooldown -= Time.fixedDeltaTime;
        }
        else if(CurrentAction == ActionAbility.Uppercut)
        {
            Reset();
        }

        if(!isLeftPressed && !isRightPressed)
        {
            Reset();
            switch (CurrentAction)
            {
                case ActionAbility.Slide:
                    isSliding = false;
                    break;
                default:
                    break;
            }
            switch (CurrentJump)
            {
                case JumpAbility.AirStabilizer:
                    isJumping = false;
                    isStabilizing = false;
                    break;
                case JumpAbility.Jetpack:
                    _isJetpackActive = false;
                    break;
                default:
                    break;
            }
        }

        if(isLeftPressed)
        {
            switch (CurrentAction)
            {
                case ActionAbility.Attack:

                    break;
                case ActionAbility.Boost:
                    isBoosting = true;
                    break;
                case ActionAbility.GrapplingHook:

                    break;
                case ActionAbility.Slide:
                    isSliding = true;
                    break;
                case ActionAbility.Uppercut:
                    isUppercutting = true;
                    break;
                default:
                    break;
            }
        }

        if (isRightPressed)
        {
            switch (CurrentJump)
            {
                case JumpAbility.AirStabilizer:
                    if (isFirstTouch) isJumping = true;
                    else isStabilizing = true;
                    break;
                case JumpAbility.DiveKick:
                    if (isFirstTouch) isJumping = true;
                    else isDiveKicking = true;
                    break;
                case JumpAbility.DoubleJump:

                    break;
                case JumpAbility.Glider:

                    break;
                case JumpAbility.Jetpack:
                    _isJetpackActive = true;
                    break;
                case JumpAbility.Jump:
                    isJumping = true;
                    break;
                default:
                    break;
            }
        }
    }

    void UpdateGroundedStatus()
    {
        grounded = Physics2D.OverlapCircle(groundCheckTransformLeft.position, 0.01f, groundCheckLayerMask) || Physics2D.OverlapCircle(groundCheckTransformRight.position, 0.01f, groundCheckLayerMask);
        if(grounded)
        {
            isFirstTouch = true;    // Should always be true upon grounded, so he can jump
            if (CurrentJump == JumpAbility.DiveKick && !isLeftPressed) player.rotation = 0;
            boostWasted = false;
            diveKickWasted = false;
            //Debug.Log("boostWasted = false");
        }

        if (Time.time % 1 == 0)
        {
            if(DEBUG) Debug.Log("Grounded: " + grounded);
        }
    }

    void Attack()
    {
        if(ScreenIsTapped() && attackCooldown <= 0)
        {
            attackCooldown = 1.0f;
            GameObject nextDestroyableObstacle = FindNearestDestroyable(transform.position);
            if(nextDestroyableObstacle != null && Vector2.Distance(killzone.transform.position, nextDestroyableObstacle.transform.position) <= 1.0f)
            {
                Debug.Log("Destroy obstacle!");
                nextDestroyableObstacle.SetActive(false);
            }
        }
    }

    /**
    * Finds the nearest "DestroyableObstacle" tagged gameobject
    **/
    GameObject FindNearestDestroyable(Vector2 currPosition)
    {
        GameObject closest = null;

        // Place all destroyable objects into an array and search for the nearest one
        GameObject[] destroyables = GameObject.FindGameObjectsWithTag("DestroyableObstacle");
        float nextObjDistance = Mathf.Infinity;

        foreach(var obstacle in destroyables)
        {
            float objDistance = Vector2.Distance(currPosition, obstacle.transform.position);
            if(objDistance < nextObjDistance)
            {
                nextObjDistance = objDistance;
                closest = obstacle;
            }
        }

        return closest;
    }

    void Boost()
    {
        if (ScreenIsTapped() && isLeftPressed && !boostWasted && boostCooldown <= 0)
        {
            //boostWasted = true;
            boostCooldown = 0.02f * boostDuration;
            //player.AddForce(new Vector2(boostForce, 0));
        }
    }

    void DiveKick()
    {
        if (grounded && ScreenIsTapped() && isRightPressed)
        {
            isFirstTouch = false;
            Jump();
            //Debug.Log("Dive...");
        }
        //did we just tap the screen for the second time?
        else if (!grounded && ScreenIsTapped() && isRightPressed && !diveKickWasted)
        {
            //Debug.Log("Kick!");
            diveKickCooldown = 0.02f * diveKickDuration;
        }
    }

    void DoubleJump()
    {
        if (ScreenIsTapped() && isRightPressed && grounded)
        {
            Jump();
            isSingleJumping = true;
        }
        else if (ScreenIsTapped() && isRightPressed && !grounded)
        {
            if(isSingleJumping && doubleJumpCooldown <= 0)
            {
                doubleJumpCooldown = 0.04f;
                isSingleJumping = false;
                player.velocity = new Vector2(movementSpeed, 0);
                player.AddForce(new Vector2(0, jumpForce));
            }
        }
    }

    void Glide()
    {
        if (isFirstTouch && ScreenIsTapped())
        {
            isFirstTouch = false;
            Jump();
        }
        else if (!isFirstTouch && isRightPressed)
        {
            Vector2 glideVelo = player.velocity;
            glideVelo.y = -0.2f * glideFallFactor;
            player.velocity = glideVelo;
        }
    }

    void Slide()
    {
        // Drop player to slide
        // (Get the size of box -> decrease position y by size/2)
        if (DEBUG_MOBILE) debuggerOutput.text = "" + Time.time + ": slide()";
        if (ScreenIsTapped())
        {
            slideFix = false;
            dropDistance = 0.5f * player.transform.localScale.x;
            Vector2 currPos = player.position;
            currPos.y -= dropDistance;

            player.position = currPos;
        }
        player.rotation = 90;
        //actionHappening = true;

        /*
		if (Input.touchCount > 0)
			player.transform.eulerAngles = new Vector3( 0, 0, 45 );
		else {
			player.transform.eulerAngles = new Vector3 ( 0, 0, 0 );
		}*/
    }

    void StabilizeY()
    {
        if (ScreenIsTapped() && isFirstTouch)
        {
            isFirstTouch = false;
            Jump();
        }
        else if(!isFirstTouch && !grounded)
        {
            Vector3 currVelo = player.velocity;
            currVelo.y = 0.65f;
            player.velocity = currVelo;
            Run();
        }
    }

    void Uppercut()
    {
        if (ScreenIsTapped() && !boostWasted)
        {
            //boostWasted = true;
            boostCooldown = 0.02f * uppercutDuration;
        }
    }

    void Jump()
    {
        //Debug.Log("" + Time.time + " RUNNING JUMP()");
        if (ScreenIsTapped())
        {
            //Debug.Log("<color=blue>" + Time.time + " SCREENISTAPPED()</color>");
            if (grounded && clickDelay <= 0)
            {
                clickDelay = 0.04f;
                player.AddForce(new Vector2(0, jumpForce));
                //Debug.Log("" + Time.time + ": JUMPING!!!!!");
                //actionHappening = false;
            }
        }
    }

    /**
    * Only adds jetpack force. See Jetpack.cs for jetpack properties.
    **/
    void ActivateJetpack()
    {
        player.AddForce(new Vector2(0, jetpackForce));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Powerup obtained
        if (collider.CompareTag("Powerup"))
        {
            UpdatePowerup(collider);
        }
        
        if (collider.CompareTag("Coin"))
        {
            Destroy(collider.gameObject);
        }
    }

    void UpdatePowerup(Collider2D powerup)
    {
        PowerupAbility obtainedPowerup = powerup.GetComponent<PowerupAbility>();
        if (obtainedPowerup.abilityType == Ability.Jump)
        {
            CurrentJump = obtainedPowerup.jumpAbility;
        }
        else
        {
            CurrentAction = obtainedPowerup.actionAbility;
        }
        Destroy(powerup.gameObject);
    }

    void Reset()
    {
        //Debug.Log("Reset");
        if (boostCooldown <= 0 && diveKickCooldown <= 0)
        {
            player.rotation = 0;
            Run();
        }
    }

    /**
     * Constantly moves player to the right
     **/
    void Run()
    {
        Vector2 newVelo = player.velocity;
        newVelo.x = movementSpeed;
        player.velocity = newVelo;
    }

    /**
    * Check for held down and not taps
    **/
    bool ScreenIsHeld(bool leftSide)
    {
        // In Android:
        // First finger = MouseButtonDown(0)
        // Second finger = MouseButtonDown(1)

        return (Input.touchCount > 0 && Input.GetMouseButton(0) && (!TouchedRightSide(Input.GetTouch(0)) && leftSide) || Input.touchCount > 0 && (TouchedRightSide(Input.GetTouch(0)) && !leftSide)
            || Input.GetMouseButtonDown(1) && (Input.touchCount > 1 && !TouchedRightSide(Input.GetTouch(1)) && leftSide) || Input.touchCount > 1 && (TouchedRightSide(Input.GetTouch(1)) && !leftSide)
            || Input.GetKey(KeyCode.LeftArrow) && leftSide || Input.GetKey(KeyCode.RightArrow) && !leftSide);
    }

    /**
     * Check for taps and not held down (Input.GetKeyDown() rather than Input.GetKey())
     **/
    bool ScreenIsTapped()
    {
        //Debug.Log("<color=orange>Checking if screenIsTapped</color>");
        //if (Input.GetKeyDown(KeyCode.LeftArrow)) return true;
        //if (Input.GetKeyDown(KeyCode.RightArrow)) return true;

        //return false;

        // In Android:
        // First finger = MouseButtonDown(0)
        // Second finger = MouseButtonDown(1)
        return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow);
    }

    /**
    * Updates the value of isLeftPressed and isRightPressed (self explanatory boolean variables)
    **/
    void TrackTouch()
    {
        bool DDEBUG = false;
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                foreach (var touch in Input.touches)
                {
                    if (!TouchedRightSide(touch))
                    {
                        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                        {
                            isLeftPressed = false;
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                        {
                            isRightPressed = false;
                        }
                    }
                }
            }
            else
            {
                isLeftPressed = isRightPressed = false;
            }
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (!Input.GetKey(KeyCode.LeftArrow))
            {
                if (DDEBUG) Debug.Log("LeftArrow UP");
                isLeftPressed = false;
            }
            if (!Input.GetKey(KeyCode.RightArrow))
            {
                if (DDEBUG) Debug.Log("RightArrow UP");
                isRightPressed = false;
            }
        }
    }
    
    /**
    * Performs action based on left or right touch
    **/
    private void RegisterTouch(bool touch)
    {
        bool DEBUG_T = false;
        if (touch == LEFT)
        {
            if(CurrentJump != JumpAbility.Jetpack) {
            isLeftPressed = true;
            // Left side was tapped
            if (DEBUG_T) Debug.Log("" + Time.time + ": " + CurrentAction);
                switch (CurrentAction)
                {
                    case ActionAbility.Attack:
                        Attack();
                        break;
                    case ActionAbility.Boost:   // DONE
                        Boost();
                        break;
                    case ActionAbility.GrapplingHook:

                        break;
                    case ActionAbility.Slide:   // DONE
                        Slide();
                        break;
                    case ActionAbility.Uppercut:    // DONE
                        Uppercut();
                        break;
                    default:
                        if (DEBUG_T) Debug.Log("Left touch error");
                        break;
                }
            }
        }
        else
        {
            isRightPressed = true;
            if (DEBUG_T) Debug.Log("" + Time.time + ": " + CurrentJump);
            // Right side was tapped
            switch (CurrentJump)
            {
                case JumpAbility.AirStabilizer: // DONE
                    StabilizeY();
                    break;
                case JumpAbility.DiveKick:  // DONE
                    DiveKick();
                    break;
                case JumpAbility.DoubleJump:    // DONE
                    DoubleJump();
                    break;
                case JumpAbility.Glider:    // DONE
                    Glide();
                    break;
                case JumpAbility.Jump:  // DONE
                    Jump();
                    break;
                case JumpAbility.Jetpack:   // DONE
                    ActivateJetpack();
                    break;
                default:
                    if(DEBUG_T) Debug.Log("Right touch error");
                    break;
            }
        }
    }

    //=== GUI ===//

    void OnGUI()
    {
        if (currentScore != null)
            UpdateScore();
    }

    void UpdateScore()
    {
        currentScore.text = "Distance: " + ((int)transform.position.x + 3) + "m";
    }

    /**
     * Check if game was touched and not UI
     **/
    private bool TouchedGameplay(Touch touch)
    {
        if (touch.position.y < (Screen.height - (Screen.height / 8)))
            return true;
        else
            return false;
    }

    private bool TouchedRightSide(Touch touch)
    {
        bool isOnScreen = false;

        if (touch.position.x > Screen.width / 2)
            isOnScreen = true;

        return isOnScreen;
    }

    private static bool LEFT = true;
    private static bool RIGHT = false;
}