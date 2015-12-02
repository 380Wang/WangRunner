using UnityEngine;
using System.Collections;

public class PowerupAbility : MonoBehaviour {

    ActionAbility _actionAbility;
    public ActionAbility actionAbility
    {
        get { return _actionAbility; }
    }

    JumpAbility _jumpAbility;
    public JumpAbility jumpAbility
    {
        get { return _jumpAbility; }
    }

    Ability chosenAbility;
    public Ability abilityType
    {
        get { return chosenAbility; }
    }

    void Awake()
    {
        chosenAbility = (Ability)Random.Range((int)Ability.Action, (int)Ability.Jump + 1);
        if (chosenAbility == Ability.Jump)
        {
            int randomIndex = Random.Range(1, (int)JumpAbility.MAX);
            _jumpAbility = (JumpAbility)randomIndex;
        }
        else
        {
            int randomIndex = Random.Range(1, (int)ActionAbility.MAX);
            _actionAbility = (ActionAbility)randomIndex;
        }
    }

	// Use this for initialization
	void Start () {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        switch (_actionAbility)
        {
            case ActionAbility.Attack:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_Attack");
                break;
            case ActionAbility.Boost:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_Boost");
                break;
            case ActionAbility.GrapplingHook:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_GrapplingHook");
                break;
            case ActionAbility.Slide:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_Slide");
                break;
            case ActionAbility.Uppercut:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_Uppercut");
                break;
            default:
                
                break;
        }

        switch (_jumpAbility)
        {
            case JumpAbility.AirStabilizer:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_AStab");
                break;
            case JumpAbility.DiveKick:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_Divekick");
                break;
            case JumpAbility.DoubleJump:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_DoubleJump");
                break;
            case JumpAbility.Glider:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_Glider");
                break;
            case JumpAbility.Jetpack:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_Jetpack");
                break;
            case JumpAbility.Jump:
                spriteRenderer.sprite = Resources.Load<Sprite>("Powerups/PU_Jump");
                break;
            default:
                
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
