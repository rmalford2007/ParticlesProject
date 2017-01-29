using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEntry : MonoBehaviour {

    public Text nameTextObject;
    public Text scoreTextObject;
    public Text initialsTextObject;
    public GameObject inputFieldObject;
    public GameObject indicatorObject;

    public string scoreName;
    public int scoreVal;
	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        if(nameTextObject)
            nameTextObject.text = scoreName;
        if(scoreTextObject)
            scoreTextObject.text = scoreVal.ToString();
        
    }

    public void SetData(string namePR, int scorePR)
    {
        scoreName = namePR;
        scoreVal = scorePR;
        UpdateObjects();

        if (inputFieldObject != null)
        {
            //request focus
            InputField theField = inputFieldObject.GetComponent<InputField>();
            if (theField != null)
            {
                theField.ActivateInputField();
                theField.Select();
            }
        }
    }

    public void UpdateObjects()
    {
        if (nameTextObject)
            nameTextObject.text = scoreName;
        if (scoreTextObject)
            scoreTextObject.text = scoreVal.ToString();
    }

    public void SaveInitials()
    {
        scoreName = initialsTextObject.text;
        UpdateObjects();
        GameManager.theManager.SetNewHighscoreName(initialsTextObject.text);
        if (inputFieldObject != null)
            Destroy(inputFieldObject);
        if (indicatorObject != null)
            Destroy(indicatorObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
