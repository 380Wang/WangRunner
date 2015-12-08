using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PowerupAbility : MonoBehaviour
{

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
        List<JumpAbility> purchasedJumps = new List<JumpAbility>();
        purchasedJumps.Add(JumpAbility.Jump);
        purchasedJumps.Add(JumpAbility.DoubleJump);


        List<ActionAbility> purchasedActions = new List<ActionAbility>();
        purchasedActions.Add(ActionAbility.Slide);

        purchasedActions.Add(ActionAbility.Attack);

        string[] jumpAbilities =
        {
            "Glider",
            "AirStabilizer",
            "DiveKick",
            "Jetpack",
        };

        string[] actionAbilities =
        {
            "GrapplingHook",
            "Boost",
            "Uppercut",
        };

        for (int i = 0; i < jumpAbilities.Length; i++)
        {
            if(PlayerPrefs.GetInt(jumpAbilities[i]) != 0)
            {
                purchasedJumps.Add((JumpAbility)Enum.Parse(typeof(JumpAbility), jumpAbilities[i]));
            }
        }

        for (int i = 0; i < actionAbilities.Length; i++)
        {
            if (PlayerPrefs.GetInt(actionAbilities[i]) != 0)
            {
                purchasedActions.Add((ActionAbility)Enum.Parse(typeof(ActionAbility), actionAbilities[i]));
            }
        }

        chosenAbility = (Ability)UnityEngine.Random.Range((int)Ability.Action, (int)Ability.Jump + 1);
        if (chosenAbility == Ability.Jump)
        {
            int randomIndex = UnityEngine.Random.Range(0, purchasedJumps.Count);
            _jumpAbility = purchasedJumps[randomIndex];
        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, purchasedActions.Count);
            _actionAbility = purchasedActions[randomIndex];
        }


    }

    // Use this for initialization
    void Start()
    {
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
    void Update()
    {

    }
}
