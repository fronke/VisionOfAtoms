using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    private Animator anim;


	void Start () {
        anim = GetComponent<Animator>();
        StartCoroutine("StartAnimator");
    }

    IEnumerator StartAnimator()
    {
        int randomAction = Mathf.RoundToInt(Random.Range(1, 2));
        float randomDelay = Random.Range(0,4);

        yield return new WaitForSeconds(randomDelay);
        anim.SetInteger("Action",randomAction);
    }

   
}
