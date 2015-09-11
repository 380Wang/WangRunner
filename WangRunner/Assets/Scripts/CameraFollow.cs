using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    /// <summary>
    /// The generic object that this camera will follow
    /// </summary>
	public GameObject targetObject;

    /// <summary>
    /// Distance between the center of the camera and where the target should be. 0 = target is at center of the screen
    /// </summary>
	private float distanceToTarget;

	// Use this for initialization
	void Start () {
		distanceToTarget = transform.position.x - targetObject.transform.position.x;
	}

	void Update () {
        //pushing the camera to its position relative to where the target is
        transform.position = new Vector3(targetObject.transform.position.x + distanceToTarget, transform.position.y, transform.position.z);
	}
}
