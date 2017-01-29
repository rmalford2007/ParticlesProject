using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnerSystem : MonoBehaviour {

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        //int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        //Rigidbody rb = other.GetComponent<Rigidbody>();
        //int i = 0;

        //while (i < numCollisionEvents)
        //{
        //    if (rb)
        //    {
        //        //collisionEvents[i].
        //        Vector3 pos = collisionEvents[i].intersection;
        //        Vector3 force = collisionEvents[i].velocity * 10;
        //        rb.AddForce(force);
        //    }
        //    i++;
        //}
    }
}
