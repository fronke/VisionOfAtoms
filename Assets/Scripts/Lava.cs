using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {

    public float hurtForce = 350f;

    private Rigidbody2D playerBody;
    private ControlsHero playerControls;

    void Start () {
        playerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerControls = GameObject.FindGameObjectWithTag("Player").GetComponent<ControlsHero>();
	}
	

	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.tag == "Player")
        {
            Vector2 direction = playerBody.position - new Vector2(transform.position.x, transform.position.y);
            playerBody.velocity = Vector3.zero;
            playerBody.AddForce(direction.normalized * hurtForce);
            playerControls.Hurt();            
        }
    }


}
