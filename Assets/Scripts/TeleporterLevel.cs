using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterLevel : MonoBehaviour {

    public string LevelToLoad;

    private bool InitSucceed = true;
    private bool IsIn = false;

    // Use this for initialization
    void Start () {
        try {
            GameObject.Find("SelectorLevel").GetComponent<SelectorLevel>().LevelToLoad = LevelToLoad;
        }
        catch (Exception e)
        {
            InitSucceed = false;
        }
    }
	

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && InitSucceed)
        {
            GetComponentInChildren<MeshRenderer>().enabled = true;
            IsIn = true;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && InitSucceed)
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            IsIn = false;
        }
    }


    void Update()
    {
        if (Input.GetAxis("Vertical") > 0 && IsIn)
        {
            SceneManager.LoadScene("Loader");
        }          
    }

}
