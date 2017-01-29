using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextUpdater_Stat : MonoBehaviour {
    public enum CastType
    {
        INT, 
        FLOAT
    }
    public Stat trackStat;
    public string prefix;
    public CastType displayCast = CastType.INT;
    Text textObject;
	// Use this for initialization
	void Start () {
        textObject = GetComponent<Text>();
        if (trackStat != null)
        {
            trackStat.StatChanged += new StatInteractionHandler(OnStatChanged);
            UpdateText(trackStat);
        }
    }

    void OnStatChanged(Stat nextScore)
    {
        UpdateText(nextScore);
    }

    void UpdateText(Stat nextScore)
    {
        if(textObject)
        {
            if(displayCast == CastType.INT)
                textObject.text = prefix + ((int)nextScore.current).ToString();
            else if(displayCast == CastType.FLOAT)
                textObject.text = prefix + ((float)nextScore.current).ToString();
        }
    }
}
