using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float boostForce = 6.0f;
    public float diveKickForce = 6.0f;
    public float movementSpeed = 3.0f;
    public float jumpForce = 200.0f;
    public float jetpackForce = 100.0f;
    public float glideFallFactor = 1.0f;
    public float uppercutForce = 12.0f;

    public Transform groundCheckTransformLeft;
    public Transform groundCheckTransformRight;
    public LayerMask groundCheckLayerMask;
    public Text currentScore;
    public JumpAbility CurrentJump = JumpAbility.Jump;
    public ActionAbility CurrentAction = ActionAbility.Slide;

    private bool grounded = false;
    private bool lastGrounded = false;
    private bool isFirstTouch = true;
    private bool isLeftPressed = false;
    private bool isRightPressed = false;
    private bool isBoosting = false;
    private bool isDiveKicking = false;
    private bool isJumping = false;
    private bool isStabilizing = false;
    private bool isSliding = false;
    private bool isUppercutting = false;
    private Rigidbody2D player;
    private float clickDelay;
    private float clickDelay2;
    private bool actionHappening = false;
    private float dropDistance;

    //===== DEBUG =====//
    private bool DEBUG = false;
    //=================//

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        // Click delay hack
        clickDelay = 0.04f;
        clickDelay2 = 0.04f;
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log ("Anchored position: " + (0.0f - titleBar.sizeDelta.y));
    }

    void FixedUpdate()
    {

        Run();
        
        // Click delay hack to prevent accidental double jumps
        clickDelay -= Time.fixedDeltaTime;
        if (CurrentJump == JumpAbility.DoubleJump) clickDelay2 -= Time.fixedDeltaTime;

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

        // Register touches
        int tCount = Input.touchCount;
        if (tCount > 0)
        {
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

        TrackTouch();
        UpdateAbilityStatus();
        UpdateGroundedStatus();
    }

    bool Sliding()
    {
        return actionHappening && ScreenIsHeld(true);
    }

    bool JustGrounded
    {
        //are we grounded now but last frame we weren't grounded?
        get
        {
            return grounded && !lastGrounded;
        }
    }

    void UpdateAbilityStatus()
    {
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
        lastGrounded = grounded;

        grounded = Physics2D.OverlapCircle(groundCheckTransformLeft.position, 0.1f, groundCheckLayerMask) || Physics2D.OverlapCircle(groundCheckTransformRight.position, 0.1f, groundCheckLayerMask);
        if(grounded)
        {
            if(CurrentJump == JumpAbility.DiveKick && !isLeftPressed) player.rotation = 0;
            isFirstTouch = true;    // Should always be true upon grounded, so he can jump
        }

        if (Time.time % 1 == 0)
        {
            if(DEBUG) Debug.Log("Grounded: " + grounded);
        }
    }

    void Boost()
    {
        Vector2 boostVelo = player.velocity;
        boostVelo.x += boostForce;
        player.velocity = boostVelo;
    }

    void DiveKick()
    {
        if (isFirstTouch && ScreenIsTapped())
        {
            isFirstTouch = false;
            Jump();
            //Debug.Log("Dive...");
        }
        //did we just tap the screen for the second time?
        else if (!isFirstTouch && isRightPressed)
        {
            //Debug.Log("Kick!");
            //isFirstTouch = true;
            Vector3 diveVelo = player.velocity;
            diveVelo.x = diveKickForce;
            diveVelo.y = -1.0f * diveKickForce;
            player.velocity = diveVelo;
            player.rotation = 45;
        }
    }

    void DoubleJump()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Grounded: " + grounded + "isFirstTouch: " + isFirstTouch);
            if (isFirstTouch)
            {
                this.Jump();
                isFirstTouch = false;
            }
            else if (!isFirstTouch && !grounded)
            {
                Debug.Log("DoubleJumping");
                isFirstTouch = true;
                if (clickDelay2 <= 0)
                {
                    clickDelay2 = 0.04f;
                    player.AddForce(new Vector2(0, jumpForce));
                }
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
            glideVelo.y = -1.0f * glideFallFactor;
            player.velocity = glideVelo;
        }
    }

    void Slide()
    {
        // Drop player to slide
        // (Get the size of box -> decrease position y by size/2)
        if (ScreenIsTapped())
        {
            dropDistance = 0.5f * transform.localScale.x;
            Vector2 currPos = player.position;
            currPos.y -= dropDistance;

            player.position = currPos;
            player.rotation = 90;
        }
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
        if (isFirstTouch && ScreenIsTapped())
        {
            isFirstTouch = false;
            Jump();
        }
        else if(!isFirstTouch && !grounded)
        {
            //Debug.Log("Stabilizing Y");
            Vector3 currVelo = player.velocity;
            currVelo.y = 0.6f;
            player.velocity = currVelo;
        }
    }

    void Uppercut()
    {
        player.velocity = new Vector2(uppercutForce, uppercutForce);
        player.rotation = -45;
        //actionHappening = true;
    }

    void Jump()
    {
        if (ScreenIsTapped())
        {
            if (grounded && clickDelay <= 0)
            {
                clickDelay = 0.04f;
                player.AddForce(new Vector2(0, jumpForce));
                //actionHappening = false;
            }
        }
    }

    void ActivateJetpack()
    {
        player.AddForce(new Vector2(0, jetpackForce));
    }

    void Reset()
    {
        player.rotation = 0;
        Run();
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
     * Check for taps and not held down
     **/
    bool ScreenIsTapped()
    {
        // In Android:
        // First finger = MouseButtonDown(0)
        // Second finger = MouseButtonDown(1)
        return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow);
    }

    void TrackTouch()
    {
        bool DDEBUG = false;
        if(Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                if (!TouchedRightSide(touch))
                {
                    if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                    {
                        isLeftPressed = false;
                        //return true;
                    }
                }
                else
                {
                    if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                    {
                        isRightPressed = false;
                        //return true;
                    }
                }
            }
        }
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

    //bool FingerLifted(bool side)
    //{
    //    bool DDEBUG = true;
    //    if(side == LEFT)
    //    {
    //        if (Input.touchCount > 0)
    //        {
    //            foreach (var touch in Input.touches)
    //            {
    //                if (!TouchedRightSide(touch))
    //                {
    //                    if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
    //                    {
    //                        // LEFT SIDE WAS LIFTED
    //                        isLeftPressed = false;
    //                        return true;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (Input.GetKeyUp(KeyCode.LeftArrow))
    //            {
    //                if (DDEBUG) Debug.Log("LeftArrow UP");
    //                isLeftPressed = false;
    //                return true;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        // RIGHT
    //        if (Input.touchCount > 0)
    //        {
    //            foreach (var touch in Input.touches)
    //            {
    //                if (TouchedRightSide(touch))
    //                {
    //                    // RIGHT SIDE WAS LIFTED
    //                    if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
    //                    {
    //                        isRightPressed = false;
    //                        return true;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (Input.GetKeyUp(KeyCode.RightArrow))
    //            {
    //                if (DDEBUG) Debug.Log("RightArrow UP");
    //                isRightPressed = false;
    //                return true;
    //            }
    //        }
    //    }

    //    return false;
    //}

    private void RegisterTouch(bool touch)
    {
        bool DEBUG_T = false;
        if (touch == LEFT)
        {
            isLeftPressed = true;
            // Left side was tapped
            if (DEBUG_T) Debug.Log("" + Time.time + ": " + CurrentAction);
            switch (CurrentAction)
            {
                case ActionAbility.Attack:

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
                    if (DEBUG_T) Debug.Log("LeftSide");
                    break;
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
                case JumpAbility.DoubleJump:    // WTF????
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
                    if(DEBUG) Debug.Log("RightSide");
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