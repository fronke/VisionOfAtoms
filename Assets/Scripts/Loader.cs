using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {


    public Text loadingText;
    private int textCount = 1;

	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateLoadingText", 0, 0.5F);
        StartCoroutine("LoadScene");
       
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);

        string levelToLoad = GameObject.Find("SelectorLevel").GetComponent<SelectorLevel>().LevelToLoad;
       
        SceneManager.LoadSceneAsync(levelToLoad);

    }

    void UpdateLoadingText()
    {
        switch (textCount)
        {
            case 1:
                loadingText.text = "Loading .";
                break;
            case 2:
                loadingText.text = "Loading ..";
                break;
            case 3:
                loadingText.text = "Loading ...";
                break;
        }
        textCount++;
        if (textCount > 3) textCount = 1;          
    }
    
}
