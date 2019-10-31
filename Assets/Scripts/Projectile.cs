using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public Vector3 direction;
    public float force = 2f;

   
    private Animator anim;
    private Rigidbody2D body;
    private Transform playerGun;
    private ControlsHero playerControls;
    private Level level;
    bool isFacingRight = false;


    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();       

        if (!isFacingRight)
         {
            direction = new Vector3(-direction.x, direction.y, 0);
        }      

        body.AddForce(direction.normalized * force);
    }

    void Update()
    {
        
    }

    public void SetOrientation(bool isFacingRight)
    {
        this.isFacingRight = isFacingRight;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag != "Enemy" && other.tag != "Bullet")
        {
            body.isKinematic = true;
            anim.SetTrigger("Reach");

            if (other.tag == "Enemy")
            {
                //other.gameObject.GetComponent<Enemy>().Hurt();
            }
        }

    }

   
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
