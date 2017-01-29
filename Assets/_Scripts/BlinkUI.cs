using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkUI : MonoBehaviour {
    public Behaviour blinkObject;
    public float blinkInterval = 1.0f;
    public bool isBlinking = true;

	// Use this for initialization
	void Start () {
        isBlinking = true;
	}

    void Awake()
    {
        if (blinkObject != null && isBlinking)
        {
            StartCoroutine(BlinkEffect());
        }
    }

    IEnumerator BlinkEffect()
    {
        while (isBlinking)
        {
            yield return new WaitForSeconds(blinkInterval);
            if(blinkObject)
            {
                blinkObject.enabled = !blinkObject.enabled;
            }
        }
        yield return null;
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
