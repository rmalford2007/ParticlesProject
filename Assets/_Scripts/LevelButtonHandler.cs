using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

    public void Restart_OnClick()
    {
        SceneManager.LoadScene("Level1");
    }

    public void MainMenu_OnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
