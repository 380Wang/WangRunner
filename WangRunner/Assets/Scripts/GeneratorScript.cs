using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorScript : MonoBehaviour {
	public GameObject[] availableLevels;
	public List<GameObject> currentLevels;

	private float screenWidthInPoints;

	// Use this for initialization
	void Start () {
		float height = 2.0f * Camera.main.orthographicSize;
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
			float roomWidth = 0.0f * room.transform.Find ("SmallGround").localScale.x;
			float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
			Debug.Log( string.Format("roomStartX: {0} screenWidth: {1}", roomStartX, screenWidthInPoints) );
			float roomEndX = roomStartX + roomWidth;

			if( roomStartX > addRoomX )
				addRooms = false;

			if( roomEndX < removeRoomX )
				roomsToRemove.Add (room);

			farthestRoomEndX = Mathf.Max (farthestRoomEndX, roomEndX);
		}

		foreach (var room in roomsToRemove) {
			currentLevels.Remove (room);
			Destroy (room);
		}

		if (addRooms)
			AddRoom (farthestRoomEndX);
	}
	
	void AddRoom( float farthestRoomEndX ){
		int randomRoomIndex = Random.Range (0, availableLevels.Length);
		GameObject room = (GameObject)Instantiate (availableLevels [randomRoomIndex]);
		float roomWidth = room.transform.Find ("SmallGround").localScale.x;
		float roomCenter = farthestRoomEndX + (roomWidth * 0.5f);

		room.transform.position = new Vector3 (roomCenter, 0, 0);
		currentLevels.Add (room);
	}
}
