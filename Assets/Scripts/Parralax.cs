using UnityEngine;
using System.Collections;

public class Parralax : MonoBehaviour {

    public float speed;

    private Camera cam;
    private float cameraPreviousX;

	// Use this for initialization
	void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraPreviousX = cam.transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {
        float cameraDeltaX = cam.transform.position.x - cameraPreviousX;
        transform.position = new Vector3(transform.position.x + (cameraDeltaX*(speed)/100), transform.position.y, transform.position.z);
        cameraPreviousX = cam.transform.position.x;
    }
}
