using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorScript : MonoBehaviour {

	public GameObject[] availableLevels;

    //how high the floor will be by default
    public float DefaultFloorY = 0;

	public List<GameObject> currentLevels;

	private float screenWidthInPoints;

	// Use this for initialization
	void Start () {
        //height of the camera
		float height = 2.0f * Camera.main.orthographicSize;

        //width/height * height = width.
		screenWidthInPoints = height * Camera.main.aspect;
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

			if( roomStartX > addRoomX )
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
			AddRoom (farthestRoomEndX);
	}
	
	void AddRoom( float farthestRoomEndX ){
		int randomRoomIndex = Random.Range (0, availableLevels.Length);
		GameObject room = (GameObject)Instantiate (availableLevels [randomRoomIndex]);
		float roomWidth = room.transform.Find ("floor").localScale.x;
		float roomCenter = farthestRoomEndX + (roomWidth * 0.5f);

		room.transform.position = new Vector3 (roomCenter, DefaultFloorY, 0);
        Debug.Log(string.Format("FloorX: {0}, FloorY: {1}, FloorHeight: {2}", farthestRoomEndX, DefaultFloorY, room.transform.Find("floor").localScale.y));
		currentLevels.Add (room);
	}
}
