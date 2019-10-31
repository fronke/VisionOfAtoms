using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float xMargin = 2f;		
    public float yMargin = 2f;      
    public float xSmooth = 2f;
    public float ySmooth = 2f;
    public Vector2 minXAndY;
    public Vector2 maxXAndY;

    public float camSize;
    private Transform player;
    private bool follow = false;

	void Start ()
	{
        camSize = GetComponent<Camera>().aspect * GetComponent<Camera>().orthographicSize;
    }

    public void SetPlayer(Transform _player)
    {
        if (!follow)
        {
            transform.position = new Vector3(_player.position.x, _player.position.y, transform.position.z);
        }

        player = _player;
        follow = true;      
    }

    public float GetSize()
    {
        return camSize;
    }

    bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }


    bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }


    void LateUpdate()
    {
        if (follow)
        {
            TrackPlayer();
        }
    }


    void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        // If the player has moved beyond the x margin...
        if (CheckXMargin())
            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

        // If the player has moved beyond the y margin...
        if (CheckYMargin())
            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = Mathf.Lerp(transform.position.y, player.position.y+1, ySmooth * Time.deltaTime);

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndY.x+camSize, maxXAndY.x-camSize);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
