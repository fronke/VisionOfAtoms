using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraUnload : MonoBehaviour 
{
	public float xMargin = 20f;		
    public float yMargin = 20f;

    public float frequency = 0.5f;

    public List<Animator> unloadedAnimators;


	void Start ()
	{
        unloadedAnimators = new List<Animator>();      

        Animator[] animators = FindObjectsOfType(typeof(Animator)) as Animator[];
        unloadedAnimators.AddRange(animators);

        InvokeRepeating("UnloadCheck", frequency, frequency);
    }

    private void UnloadCheck()
    {
        float minX = this.transform.position.x - xMargin;
        float maxX = this.transform.position.x + xMargin;
        float maxY = this.transform.position.y + yMargin;

        foreach (Animator animator in unloadedAnimators)
        {
            if (animator.transform.position.x < minX || animator.transform.position.x > maxX || animator.transform.position.y > maxY)
            {
                animator.enabled = false;
            }
            else
            {
                animator.enabled = true;
            }
        }       

    }

   
}
