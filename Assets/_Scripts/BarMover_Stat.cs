using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarMover_Stat : MonoBehaviour {
    //Controls the width of a bar, based on the current stat value to max stat value percentage

    public Image movingBar;
    public Stat trackStat;
    float maxWidth;
	// Use this for initialization
	void Start () {
        if (!movingBar)
            movingBar = GetComponent<Image>();
        trackStat.StatChanged += new StatInteractionHandler(OnStatChanged);
        if (movingBar)
            maxWidth = movingBar.rectTransform.sizeDelta.x;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnStatChanged(Stat s)
    {
        float widthPercentage = s.percentage();
        movingBar.rectTransform.sizeDelta = new Vector2(maxWidth * widthPercentage , movingBar.rectTransform.sizeDelta.y);
    }
}
