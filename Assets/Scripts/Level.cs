using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    public GameObject heroBlue;
    public GameObject heroPink;

    public CameraFollow cam;

    public int activeHero = 0;


    void Awake()
    {
        DontDestroyOnLoad(GameObject.Find("SelectorLevel"));
    }

    void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        cam.SetPlayer(heroBlue.transform);
    }

	void Update () {
        
        if (Input.GetButtonDown("Action"))
        {
            if (activeHero == 0)
            {
                heroBlue.GetComponent<ControlsHero>().enabled = false;
                heroPink.GetComponent<ControlsHero>().enabled = true;
                cam.SetPlayer(heroPink.transform);
            }
            else if (activeHero == 1)
            {
                heroBlue.GetComponent<ControlsHero>().enabled = true;
                heroPink.GetComponent<ControlsHero>().enabled = false;
                cam.SetPlayer(heroBlue.transform);
            }

            activeHero++;
            if (activeHero > 1)
            {
                activeHero = 0;
            }
        }       

    }

    public GameObject GetCurrentHero()
    {
        switch (activeHero)
        {
            case 0:
                return heroBlue;
            case 1:
                return heroPink;
            default:
                return null;
        }        
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
