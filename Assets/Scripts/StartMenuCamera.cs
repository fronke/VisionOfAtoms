using UnityEngine;
using System.Collections;

public class StartMenuCamera : MonoBehaviour {

    private StartMenu Level;
	

	void Start () {
        Level = GameObject.Find("Level").GetComponent<StartMenu>();
	}
	
	public void FallingEnd()
    {
        Level.StartGame();
    }
	
}
