using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plateform : MonoBehaviour
{
    public float minimum;
    public float maximum;
    public float speed;
    public bool isMovingVertically = false;
    public bool canFalling = false;
    public bool canRotate = false;
    private bool isFalling = false;

    private GameObject originalObject;
    private bool flipped = false;
    private Rigidbody2D body;
    private Animator anim;

    void Start()
    {
        if (canFalling)
        {
            originalObject = (GameObject) Instantiate(gameObject, transform.position, Quaternion.identity);
            originalObject.SetActive(false);
            originalObject.transform.SetParent(transform.parent);
        }

        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (!canRotate)
        {
            GetNewDestination();
        }
        
        Physics.IgnoreLayerCollision(11, 11, true);
    }


    void LateUpdate()
    {
        if (!isFalling)
        {
            if (!canRotate)
            {
                float currentPosition;

                if (isMovingVertically)
                {
                    currentPosition = this.transform.localPosition.y;
                }
                else
                {
                    currentPosition = this.transform.localPosition.x;
                }

                if ((flipped && (currentPosition - minimum) < 0.1f) || (!flipped && (maximum - currentPosition) < 1f))
                {
                    GetNewDestination();
                }
            }
            else
            {
                transform.Rotate(Vector3.forward, Time.deltaTime * speed, Space.Self);              
            }
        }
    }


    void GetNewDestination()
    {
        Vector3 newVelocity;       

        if (isMovingVertically)
        {
            if (flipped)
            {
                newVelocity = new Vector2(0, 1);
            }
            else
            {
                newVelocity = new Vector2(0, -1);
            }            
        }
        else
        {
            if (flipped)
            {
                newVelocity = new Vector2(1, 0);
            }
            else
            {
                newVelocity = new Vector2(-1, 0);
            }
        }

        flipped = !flipped;
        body.velocity = newVelocity * speed;        
    }


    void OnCollisionEnter2D(Collision2D coll)
    {    
        if (canFalling && coll.gameObject.tag == "Player")
        {
            anim.SetTrigger("Shake");
            StartCoroutine(FallIn(1.5F));
        }

    }

    IEnumerator FallIn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        anim.SetTrigger("Fall");
        isFalling = true;
        body.isKinematic = false;

        yield return new WaitForSeconds(3f);
        originalObject.SetActive(true);
    }




}
