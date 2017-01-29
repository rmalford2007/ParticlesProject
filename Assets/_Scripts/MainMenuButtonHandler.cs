using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenuButtonHandler : MonoBehaviour {

    
    public GameObject playGameBtnObject;
    public GameObject highScoreBtnObject;
    public GameObject quitGameBtnObject;
    public GameObject highScoreTableObject;
    public GameObject clearScoreObject;
    public GameObject confirmObject;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayGameOnClick()
    {
        SceneManager.LoadScene("Level1");
    }

    public void HighscoresOnClick()
    {
        highScoreTableObject.SetActive(!highScoreTableObject.activeSelf);
        clearScoreObject.SetActive(!clearScoreObject.activeSelf);
    }

    public void ClearHighscoresOnClick()
    {
        playGameBtnObject.SetActive(!playGameBtnObject.activeSelf);
        highScoreBtnObject.SetActive(!highScoreBtnObject.activeSelf);
        quitGameBtnObject.SetActive(!quitGameBtnObject.activeSelf);
        clearScoreObject.SetActive(!clearScoreObject.activeSelf);
        confirmObject.SetActive(!confirmObject.activeSelf);
    }

    public void ConfirmYesOnClick()
    {
        ClearHighscores();

        ClearHighscoresOnClick();
    }

    public void ConfirmNoOnClick()
    {
        //No, don't clear the highscore
        ClearHighscoresOnClick();
    }

    void ClearHighscores()
    {
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey("ScoreName" + i.ToString()))
            {
                PlayerPrefs.DeleteKey("ScoreName" + i.ToString());
            }
            if (PlayerPrefs.HasKey("ScoreVal" + i.ToString()))
            {
                PlayerPrefs.DeleteKey("ScoreVal" + i.ToString());
            }
        }

        int childCount = highScoreTableObject.transform.childCount;
        for(int j = 2; j < childCount; j++)
        {
            Transform nextChildTransform = highScoreTableObject.transform.GetChild(j);
            if (nextChildTransform != null)
                Destroy(nextChildTransform.gameObject);
        }
        
    }

    public void QuitGameOnClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
        #else
            Application.Quit();
        #endif
    }
}
