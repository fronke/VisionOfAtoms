using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

    public Animator UiAnim;
    public Animator CameraAnimator;
    public Animator BackgroundAnimator;


    void Awake()
    {
        DontDestroyOnLoad(GameObject.Find("SelectorLevel"));
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Start")) {

            UiAnim.SetTrigger("startPressed");
            CameraAnimator.SetTrigger("startPressed");
            BackgroundAnimator.SetTrigger("startPressed");
        }
	}

    public void StartGame()
    {
       
        SceneManager.LoadScene("Loader");
    }

}
