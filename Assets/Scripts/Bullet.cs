using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float bulletSpeed = 2f;

    private bool isMoving = false;
    private Animator anim;
    private Rigidbody2D body;
    private Transform playerGun;
    private ControlsHero playerControls;
    private Level level;

    void Start () {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();      
        level = GameObject.Find("Level").GetComponent<Level>();
        playerGun = level.GetCurrentHero().transform.FindChild("playerGun").GetComponent<Transform>();
        playerControls = level.GetCurrentHero().GetComponent<ControlsHero>();
    }

    void Update()
    {
      if (!isMoving)
        {
            transform.position = playerGun.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player" && other.tag != "Bullet")
        {
            body.velocity = Vector2.zero;
            anim.SetTrigger("Reach");

            if (other.tag == "Enemy")
            {
                other.gameObject.GetComponent<Enemy>().Hurt();
            }
        }
        
    }

    public void Appeared()
    {
        if (playerControls.IsFacingRight())
        {
            body.velocity = Vector2.right * bulletSpeed;
        }
        else
        {
            body.velocity = Vector2.left * bulletSpeed;
        }      
        isMoving = true;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
