using UnityEngine;
using System.Collections;


public class AnimationTrigger : MonoBehaviour {

    public string TriggerName;
    public float TriggerEvery = 2f;
    public float InitialDelay = 2f;

    private Animator anim;
  

    void Start () {
        anim = GetComponent<Animator>();
        
        InvokeRepeating("Trigger", Random.value * InitialDelay, TriggerEvery);
	}
	
    void Trigger()
    {
        anim.SetTrigger(TriggerName);
    }


}
