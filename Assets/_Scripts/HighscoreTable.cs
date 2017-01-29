using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour {

    public GameObject defaultScoreEntry;
    public GameObject promptScoreEntry;
   // bool isInitialized = false;
	// Use this for initialization
	void Start () {
		//the public gameobjects require hookup in inspector
        if(defaultScoreEntry == null || promptScoreEntry == null)
        {
            Debug.Log("Highscore table is missing prefab entries for scores.");
        }
	}

    void Awake()
    {
        //On awake fill the highscore table based on what is in the highscore list in the game manager
        Init();
    }

    void Init()
    {
        if (GameManager.theManager != null && GameManager.theManager.GetHighList() != null)
        {

            List<GameManager.Score> highScoreList = GameManager.theManager.GetHighList();

            for (int i = 0; i < highScoreList.Count; i++)
            {
                if (highScoreList[i].newHighscoreFlag)
                {
                    //new score
                    AddNewScoreEntry(i, highScoreList[i].scoreVal);
                }
                else
                {
                    //old score
                    AddScoreEntry(i, highScoreList[i].scoreName, highScoreList[i].scoreVal);
                }
            }
        }
    }

    void AddScoreEntry(int pos, string namePR, int scorePR)
    {
        //This is a regular highscore entry, nothing new here, just list it
        GameObject createdScore = Instantiate(defaultScoreEntry,transform,false);

        //Set the anchored transform y position, to be offset by height + 1 for spacing
        RectTransform createdTransform = createdScore.GetComponent<RectTransform>();
        Vector2 newPosition = createdTransform.anchoredPosition;
        newPosition.y = pos * (-(createdTransform.sizeDelta.y+1)); //the height of score entries, assume new entries and old entries are same height, should be -(22 + 1)
        createdTransform.anchoredPosition = newPosition;

        ScoreEntry scoreScript = createdScore.GetComponent<ScoreEntry>();
        if(scoreScript != null)
        {
            scoreScript.SetData(namePR, scorePR);
        }
    }

    void AddNewScoreEntry(int pos, int scorePR)
    {
        //This is a new highscore entry, needs focus control to add initials, also blink the the control to indicate to the player to add their initials
        GameObject createdScore = Instantiate(promptScoreEntry, transform, false);

        //Set the anchored transform y position, to be offset by height + 1 for spacing
        RectTransform createdTransform = createdScore.GetComponent<RectTransform>();
        Vector2 newPosition = createdTransform.anchoredPosition;
        
        newPosition.y = pos * (-(createdTransform.sizeDelta.y + 1)); //the height of score entries, assume new entries and old entries are same height, should be -(22 + 1)
        createdTransform.anchoredPosition = newPosition;

        ScoreEntry scoreScript = createdScore.GetComponent<ScoreEntry>();
        if (scoreScript != null)
        {
            scoreScript.SetData("", scorePR);
        }
    }
	
	// Update is called once per frame
	void Update () {

	}
}
