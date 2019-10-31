using UnityEngine;
using System.Collections;

public class BackgroundWander : MonoBehaviour {


    public Vector2 minXY;
    public Vector2 maxXY;
    public float speed;
    public bool isFlipping;
    public bool isFacingRight;
    public bool isVerticalLocked;
    public bool isHorizontalLocked;

    private Vector2 destination;

    void Start()
    {
        GetNewDestination();
    }

    void LateUpdate()
    {
        float step = speed * Time.deltaTime;       

        transform.localPosition = Vector2.MoveTowards(transform.localPosition, destination, step);

        if (Vector3.Distance(destination, transform.localPosition) < 0.1f)
        {
            isFacingRight = !isFacingRight;

            if (isFlipping)
            {
                Flip();
            }

            GetNewDestination();
        }       
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void GetNewDestination()
    {
        float destinationX;
        if (!isHorizontalLocked)
        {      
            if (isFacingRight)
            {
                destinationX = maxXY.x;
            }
            else
            {
                destinationX = minXY.x;
            }
        }
        else
        {
            destinationX = transform.localPosition.x;
        }

        float destinationY;
        if (!isHorizontalLocked)
        {
            if (isVerticalLocked)
            {
                destinationY = transform.localPosition.y;
            }
            else
            {
                destinationY = Random.Range(minXY.y, maxXY.y);
            }
        }
        else
        {
            if (isFacingRight)
            {
                destinationY = maxXY.y;
            }
            else
            {
                destinationY = minXY.y;
            }
        }

        destination = new Vector2(destinationX, destinationY);
    }
}
