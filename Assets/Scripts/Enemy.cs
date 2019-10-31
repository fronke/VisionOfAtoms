using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Object Projectile;

    public float walkingSpeed = 0.5f;
    public float runningSpeed = 2.5f;
    public int HP = 15;
    public float fieldOfView = 4f;

    private Animator animator;
    private Transform groundCheck;
    private Transform frontCheck;
    public Transform projectileStart;
    public bool attacking = false;
    public bool freeze = false;
    public bool dead = false;
    public bool sawHero = false;
    public bool isFacingRight = false;

    Coroutine forgetHeroCoroutine;

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        groundCheck = transform.Find("groundCheck").transform;
        frontCheck = transform.Find("frontCheck").transform;
    }

    void FixedUpdate()
    {
        if (!dead) {
            // Create an array of all the colliders in front of the enemy.
            Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position);
            Collider2D[] bottomHits = Physics2D.OverlapPointAll(groundCheck.position);

            //Check each of the colliders.
            foreach (Collider2D c in frontHits)
            {
                if (c.tag == "Player")
                {
                    if (!attacking)
                    {
                        animator.SetTrigger("Attack");
                        c.gameObject.GetComponent<ControlsHero>().Hurt();
                        break;
                    }
                }
                else
                {
                    Flip();
                    break;
                }
            }

            bool plateformEnd = true;
            foreach (Collider2D c in bottomHits)
            {
                if (c.tag == "Ground")
                {
                    plateformEnd = false;                   
                    break;
                }
            }

            if (plateformEnd)
            {
                Flip();
            }

            Vector2 direction = Vector2.left;
            if (isFacingRight)
            {
                direction = Vector2.right;
            }
            RaycastHit2D hit = Physics2D.Raycast(frontCheck.position, direction, fieldOfView);
            if (hit.collider != null && hit.collider.tag == "Player")
            {

                sawHero = true;
                if (forgetHeroCoroutine != null)
                {
                    StopCoroutine(forgetHeroCoroutine);
                }
                forgetHeroCoroutine = StartCoroutine("ForgetHero");

                if (Vector3.Distance(transform.position, hit.transform.position) > 3) {
                    Shoot();
                }
            }


            float speed = sawHero ? runningSpeed : walkingSpeed;
            if (freeze)
            {
                speed = 0;
            }          

            // Set the enemy's velocity to moveSpeed in the x direction.
            GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localScale.x * speed, GetComponent<Rigidbody2D>().velocity.y);
            animator.SetFloat("Speed", speed);

            // If the enemy has zero or fewer hit points and isn't dead yet...
            if (HP <= 0 && !dead)
            {
                Death();
            }
        }
    }

    public IEnumerator ForgetHero()
    {
        yield return new WaitForSeconds(2f);
        sawHero = false;
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    public void ShootProjectile()
    {
       
        if (isFacingRight)
        {        
            (Instantiate(Projectile, projectileStart.position, Quaternion.Euler(new Vector3(0, 0, 0f))) as GameObject).GetComponent<Projectile>().SetOrientation(isFacingRight);
        }
        else
        {
            (Instantiate(Projectile, projectileStart.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as GameObject).GetComponent<Projectile>().SetOrientation(isFacingRight);
        }
    }

    public void StartFreeze()
    {
        this.freeze = true;
    }
    public void StopFreeze()
    {
        this.freeze = false;
    }

    public void StartAttack()
    {
        this.attacking = true;
    }
    public void StopAttack()
    {
        this.attacking = false;
    }


    public void Hurt()
    {
        if (!dead)
        {
            HP--;
            animator.SetTrigger("Hurt");
        }
    }

    void Death()
    {      
        // Set dead to true.
        dead = true;

        GetComponent<Rigidbody2D>().isKinematic = true;
        // Find all of the colliders on the gameobject and set them all to be triggers.
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }
        animator.SetFloat("Speed", 0);
        animator.SetTrigger("Cured");
    }


    public void Flip()
    {
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
        isFacingRight = !isFacingRight;
    }

    
}