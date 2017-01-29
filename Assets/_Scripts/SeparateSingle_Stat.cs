using UnityEngine;
using System.Collections;

public class SeparateSingle_Stat : MonoBehaviour {

    public GameObject[] singlesArray;
    public Stat trackStat;

	// Use this for initialization
	void Start () {
        if (trackStat)
            trackStat.StatChanged += new StatInteractionHandler(OnStatChanged);
        SyncDisplay(trackStat);
	}

    void OnStatChanged(Stat s)
    {
        SyncDisplay(s);
    }

    void SyncDisplay(Stat s)
    {
        //sync the displayed lives to the stat current value
        for(int i = 0; i < singlesArray.Length; i++)
        {
            if (i >= s.current)
                singlesArray[i].SetActive(false);
            else
                singlesArray[i].SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
