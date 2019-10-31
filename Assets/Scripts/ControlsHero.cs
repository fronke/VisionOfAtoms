using UnityEngine;
using System.Collections;

    public class ControlsHero : MonoBehaviour
{

    public GameObject bullet;

    public float moveForce;    
    public float initialJumpForce;    
    public float extraJumpForce;
    public float doubleJumpForce;

    public float maxExtraJumpTime;
    public float maxMoveSpeed;
    public float maxSprintSpeed;
    public float maxFallSpeed;

    private bool playerJump = false;
    private bool playerJumping = false;
    private bool playerDoubleJump = false;
    private bool playerDoubleJumped = false;
    private bool jumpedDuringSprint = false;
    private float jumpTimer = 0f;

    private bool isGrounded = false;
    private float horizontalInput = 0f;

    private Animator anim;
    private Rigidbody2D body;
    private Transform groundChecker;
    private Transform gun;

    private Vector3 pointUp;
    private bool facingRight = true;
    bool isClimbing = false;
    Rope currentRope = null;

    bool col1 = false;
    bool col2 = false;

    void Start()
    {
        groundChecker = transform.Find("groundCheck");
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        gun = this.transform.Find("playerGun").GetComponent<Transform>();
    }


    void Update()
    {
        isGrounded = Physics2D.Linecast(transform.position, groundChecker.position, 1 << LayerMask.NameToLayer("Ground"));

        if (isGrounded)
        {
            playerDoubleJumped = false;
            // jumpedDuringSprint = false; / not good, stop it before it goes

            if (!Input.GetButton("Sprint"))
             {
                jumpedDuringSprint = false;
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerJump = true;
            playerJumping = true;
            jumpTimer = Time.time;
        }
        else if (Input.GetButtonDown("Jump") && !isGrounded && !playerDoubleJumped)
        {
            playerDoubleJumped = true;
            playerDoubleJump = true;
        }

        if (Input.GetButtonUp("Jump") || Time.time - jumpTimer > maxExtraJumpTime)
        {
            playerJumping = false;
        }
      
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));

        UpdateClimb();

        // Gun managment
        if (Input.GetButtonDown("Fire"))
        {
            anim.SetTrigger("Shoot");

            if (facingRight)
            {
                Instantiate(bullet, gun.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
            else
            {
                Instantiate(bullet, gun.position, Quaternion.Euler(new Vector3(0, 0, 180f)));
            }
        }
    }

    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        /*if (isGrounded)
        {
            Vector2 platformDirection = GetCurrentPlatformDirection(groundChecker);
            body.velocity = new Vector2(platformDirection.x, body.velocity.y);
        }*/                

        //If our player is holding the sprint button, we've held down the button for a while, and we're grounded...
        //OR our player jumped while we were already sprinting...
        if ((Input.GetButton("Sprint") && isGrounded) || jumpedDuringSprint)
        {
            //... then sprint
          //GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalInput * moveForce  * Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
            
            if (horizontalInput * body.velocity.x < maxSprintSpeed)
            {
                body.AddForce(Vector2.right * horizontalInput * moveForce);
            }        
            
            // If the player's horizontal velocity is greater than the maxSpeed...
            if (Mathf.Abs(body.velocity.x) > maxSprintSpeed)
            {
                // ... set the player's velocity to the maxSpeed in the x axis.
                body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxSprintSpeed, body.velocity.y);
            }
            

            //If our player jumped during our sprint...
            if (playerJump)
            {
                // keep the momentum
                jumpedDuringSprint = true; 
            }
        }
        else
        {
            //GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalInput * moveForce/2 * Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
            
            if (horizontalInput * body.velocity.x < maxMoveSpeed)
            {
                body.AddForce(Vector2.right * horizontalInput * moveForce);
            }
           
            // If the player's horizontal velocity is greater than the maxSpeed...
            if (Mathf.Abs(body.velocity.x) > maxMoveSpeed)
            {
                // ... set the player's velocity to the maxSpeed in the x axis.
                body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxMoveSpeed, body.velocity.y);
            }     
            
        }

        if (playerJump)
        {
            body.AddForce(new Vector2(0, initialJumpForce));
            playerJump = false;
            anim.SetTrigger("Jump");
        }
        else if (playerJumping)
        {        
            body.AddForce(new Vector2(0, extraJumpForce));
        }

        if (playerDoubleJump)
        {
            body.AddForce(new Vector2(0, doubleJumpForce));
            playerDoubleJump = false;
        }

        if ((horizontalInput > 0 && !facingRight) || (horizontalInput < 0 && facingRight))
        {
            Flip();
        }
    }


    public void UpdateClimb()
    {
    
        if (isClimbing)
        {

            if (Input.GetButtonDown("Jump") || Input.GetAxis("Vertical") < 0)
            {
                body.isKinematic = false;              
                currentRope.ReleaseRope();
            }
            else if (Input.GetAxis("Vertical") > 0 && currentRope != null)
            {
                body.isKinematic = true;

                if (!currentRope.IsCatched())
                {                    
                    currentRope.CatchRope(transform.position);
                }
                
                Vector3 nextPosition = currentRope.GetNextPositionUp(transform.position);               

                if (!nextPosition.Equals(Vector3.zero)) {
                    Debug.Log(nextPosition);
                     transform.position = Vector3.MoveTowards(transform.position, nextPosition, 3 * Time.deltaTime);
                }
                else
                {
                    body.isKinematic = false;                   
                    currentRope.ReleaseRope();
                    currentRope = null;
                    isClimbing = false;
                }                  
            }           
               
               
        }
   
    }
    
    //enter the trigger
    void OnTriggerEnter2D(Collider2D col)
    {       
        if (col.gameObject.tag == "Ladder")
        {
           
            if (col1 == true)
            {
                col2 = true;
            }
            else
            {
                col1 = true;
            }           

            isClimbing = true;
            currentRope = col.gameObject.GetComponent<Rope>();
        }
    }   
    

    //exit the trigger     
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
        {
         
            if (col2 == false)
            {
                col1 = false;
            }
            else
            {
                col2 = false;
            }

            if (!col1 && !col2)
            {
                isClimbing = false;
                if (currentRope != null)
                {
                    currentRope.ReleaseRope();
                }
                currentRope = null;

                body.isKinematic = false;               
            }            
        }
    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    Vector2 GetCurrentPlatformDirection(Transform checkWith)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, checkWith.position, 1 << LayerMask.NameToLayer("Ground"));
        if (hit.transform != null)
        {
            Rigidbody2D cp = hit.transform.GetComponent<Rigidbody2D>();
            if (cp != null)
            {
                return cp.velocity;
            }
        }
        return new Vector2(0, 0);
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public void Hurt()
    {
        anim.SetTrigger("Hurt");
    }
}