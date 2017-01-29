using UnityEngine;
using System.Collections;

public class DamageOnHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision(GameObject other)
    {
        //When a bullet collides with a particle update the score
        GameManager.theManager.IncrementScore(1);
        Destroy(gameObject);
    }

}
