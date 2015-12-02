using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIScript : MonoBehaviour {
    public Image actionAbility;
    public Image jumpAbility;
    public Sprite jetpackSprite;
    PlayerController player;
    PowerupAbility powerup;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        powerup = new PowerupAbility();
	}

    void OnGUI()
    {
        switch (player.CurrentAction)
        {
            case ActionAbility.Attack:
                actionAbility.sprite = Resources.Load<Sprite>("Powerups/PU_Attack");
                break;
            case ActionAbility.Boost:
                actionAbility.sprite = Resources.Load<Sprite>("Powerups/PU_Boost");
                break;
            case ActionAbility.GrapplingHook:
                actionAbility.sprite = Resources.Load<Sprite>("Powerups/PU_GrapplingHook");
                break;
            case ActionAbility.Slide:
                actionAbility.sprite = Resources.Load<Sprite>("Powerups/PU_Slide");
                break;
            case ActionAbility.Uppercut:
                actionAbility.sprite = Resources.Load<Sprite>("Powerups/PU_Uppercut");
                break;
            default:

                break;
        }

        switch (player.CurrentJump)
        {
            case JumpAbility.AirStabilizer:
                jumpAbility.sprite = Resources.Load<Sprite>("Powerups/PU_AStab");
                break;
            case JumpAbility.DiveKick:
                jumpAbility.sprite = Resources.Load<Sprite>("Powerups/PU_Divekick");
                break;
            case JumpAbility.DoubleJump:
                jumpAbility.sprite = Resources.Load<Sprite>("Powerups/PU_DoubleJump");
                break;
            case JumpAbility.Glider:
                jumpAbility.sprite = Resources.Load<Sprite>("Powerups/PU_Glider");
                break;
            case JumpAbility.Jetpack:
                jumpAbility.sprite = Resources.Load<Sprite>("Powerups/PU_Jetpack");
                break;
            case JumpAbility.Jump:
                jumpAbility.sprite = Resources.Load<Sprite>("Powerups/PU_Jump");
                break;
            default:

                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
