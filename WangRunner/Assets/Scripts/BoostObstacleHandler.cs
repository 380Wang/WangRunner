using UnityEngine;

public class BoostObstacleHandler : MonoBehaviour {
    public float closeSpeedFactor = 1.0f;

    private float topObstacleBeginY;
    private float bottomObstacleBeginY;
    private float startingDistance;
    private float currDistance;
    private float minGapDistance;
    private bool openGap = true;

    Transform topObstacle;
    Transform bottomObstacle;
    Transform topObstacleEdge;
    Transform bottomObstacleEdge;
    float playerHeight;

	// Use this for initialization
	void Start () {
        topObstacle = transform.Find("CeilingBoostObstacle");
        bottomObstacle = transform.Find("FloorBoostObstacle");
        topObstacleEdge = topObstacle.Find("Obstacle").Find("Edge");
        bottomObstacleEdge = bottomObstacle.Find("Obstacle").Find("Edge");
        topObstacleBeginY = topObstacleEdge.position.y;
        bottomObstacleBeginY = bottomObstacleEdge.position.y;
        startingDistance = topObstacleBeginY - bottomObstacleBeginY;

        playerHeight = GameObject.FindGameObjectWithTag("Player").transform.localScale.y;
        minGapDistance = playerHeight;
	}
	
	void FixedUpdate () {
        currDistance = topObstacleEdge.position.y - bottomObstacleEdge.position.y;

        if (openGap)
        {
            CloseGap();
            if(currDistance <= minGapDistance)
            {
                openGap = false;
            }
        }
        else
        {
            OpenGap();
            if(currDistance >= startingDistance)
            {
                openGap = true;
            }
        }
    }

    void CloseGap()
    {
        Vector3 topObstaclePosition = topObstacle.position;
        topObstaclePosition.y -= 0.01f * closeSpeedFactor;
        topObstacle.position = topObstaclePosition;
        Vector3 bottomObstaclePosition = bottomObstacle.position;
        bottomObstaclePosition.y += 0.01f * closeSpeedFactor;
        bottomObstacle.position = bottomObstaclePosition;
    }

    void OpenGap()
    {
        Vector3 topObstaclePosition = topObstacle.position;
        topObstaclePosition.y += 0.01f * closeSpeedFactor;
        topObstacle.position = topObstaclePosition;
        Vector3 bottomObstaclePosition = bottomObstacle.position;
        bottomObstaclePosition.y -= 0.01f * closeSpeedFactor;
        bottomObstacle.position = bottomObstaclePosition;
    }
}
