using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorScript : MonoBehaviour {

    public GameObject[] attackLevels;
	public GameObject[] basicLevels;
    public GameObject[] boostLevels;
    public GameObject[] diveKickLevels;
    public GameObject[] gliderLevels;
    public GameObject[] slideLevels;
    public GameObject[] stabilizerLevels;
    public GameObject[] uppercutLevels;

    //how high the level prefab will be by default
    public float DefaultFloorY = 0;

    //the y axis of the floor of the level prefab by default
    public float DefaultObstacleY = -3.5f;

	public List<GameObject> currentLevels;

	private float screenWidthInPoints;
    private GameObject[] availableLevels = new GameObject[2];
    private PlayerController player;

    //===== DEBUG =====//
    bool DEBUG = true;
    //================//

    // Use this for initialization
    void Start () {
        //height of the camera
		float height = 2.0f * Camera.main.orthographicSize;

        //width/height * height = width.
		screenWidthInPoints = height * Camera.main.aspect;
        player = GetComponent<PlayerController>();
	}

	void FixedUpdate(){
		GenerateRoomIfRequired ();
	}

	void GenerateRoomIfRequired(){
		List<GameObject> roomsToRemove = new List<GameObject>();
		bool addRooms = true;
		float playerX = transform.position.x;
		float removeRoomX = playerX - screenWidthInPoints;
		float addRoomX = playerX + screenWidthInPoints;
		float farthestRoomEndX = 0;

		foreach (var room in currentLevels) {
			float roomWidth = room.transform.Find ("floor").localScale.x;
			float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
			float roomEndX = roomStartX + roomWidth;

			if( roomEndX > addRoomX )
				addRooms = false;

			if( roomEndX < removeRoomX )
				roomsToRemove.Add (room);

			farthestRoomEndX = Mathf.Max (farthestRoomEndX, roomEndX);
		}
        for (int i = 0; i < roomsToRemove.Count; i++)
        {
            currentLevels.Remove(roomsToRemove[i]);
            Destroy(roomsToRemove[i]);
        }

        if (addRooms)
        {
            AddRoom(farthestRoomEndX);
        }
	}
	
	void AddRoom( float farthestRoomEndX ){

        int randomIndex = 0;

        if (player.CurrentJump != JumpAbility.Jetpack)
        {
            // Left action
            switch (player.CurrentAction)
            {
                case ActionAbility.Attack:
                    randomIndex = Random.Range(0, attackLevels.Length);
                    availableLevels[0] = attackLevels[randomIndex];
                    break;
                case ActionAbility.Boost:
                    randomIndex = Random.Range(0, boostLevels.Length);
                    availableLevels[0] = boostLevels[randomIndex];
                    break;
                case ActionAbility.GrapplingHook:
                    randomIndex = Random.Range(0, basicLevels.Length);
                    availableLevels[0] = basicLevels[randomIndex];
                    break;
                case ActionAbility.Slide:
                    randomIndex = Random.Range(0, slideLevels.Length);
                    availableLevels[0] = slideLevels[randomIndex];
                    break;
                case ActionAbility.Uppercut:
                    randomIndex = Random.Range(0, uppercutLevels.Length);
                    availableLevels[0] = uppercutLevels[randomIndex];
                    break;
                default:
                    Debug.Log("Oh no...");
                    break;
            }
        }
        else
        {
            randomIndex = Random.Range(0, basicLevels.Length);
            availableLevels[0] = basicLevels[0];
        }
        if (availableLevels[0] == null)
        {
            if (DEBUG) Debug.Log("Level generate fail.");
            return;
        }
        // Right action
        switch (player.CurrentJump)
        {
            case JumpAbility.AirStabilizer:
                randomIndex = Random.Range(0, basicLevels.Length);
                availableLevels[1] = stabilizerLevels[randomIndex];
                break;
            case JumpAbility.DiveKick:
                randomIndex = Random.Range(0, diveKickLevels.Length);
                availableLevels[1] = diveKickLevels[randomIndex];
                break;
            case JumpAbility.DoubleJump:
                randomIndex = Random.Range(0, basicLevels.Length);
                availableLevels[1] = basicLevels[randomIndex];
                break;
            case JumpAbility.Glider:
                randomIndex = Random.Range(0, gliderLevels.Length);
                availableLevels[1] = gliderLevels[randomIndex];
                break;
            case JumpAbility.Jetpack:
                randomIndex = Random.Range(0, basicLevels.Length);
                availableLevels[1] = basicLevels[randomIndex];
                break;
            case JumpAbility.Jump:
                randomIndex = Random.Range(0, basicLevels.Length);
                availableLevels[1] = basicLevels[randomIndex];
                break;
            default:
                Debug.Log("Dieeee!!!");
                break;
        }

		int randomRoomIndex = Random.Range (0, availableLevels.Length);
		GameObject room = (GameObject)Instantiate (availableLevels [randomRoomIndex]);
		float roomWidth = room.transform.Find ("floor").localScale.x;
		float roomCenter = farthestRoomEndX + (roomWidth * 0.5f);

		room.transform.position = new Vector3 (roomCenter - 0.2f, DefaultFloorY, 0);
        //Debug.Log(string.Format("FloorX: {0}, FloorY: {1}, FloorHeight: {2}", farthestRoomEndX, DefaultFloorY, room.transform.Find("floor").localScale.y));
		currentLevels.Add (room);
	}
}
