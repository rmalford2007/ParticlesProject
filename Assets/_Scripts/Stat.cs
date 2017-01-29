using UnityEngine;
using System.Collections;

/****************************************
 * Throughout many of my projects I end up using some sort of 
 * stat class to track linear items like health, armor, lives, 
 * score, and more.I expose event delegate functions to allow 
 * other objects to subscribe and "watch" them for changes.
 * *************************************/

public delegate void StatInteractionHandler(Stat s);

public class Stat : MonoBehaviour
{
    public event StatInteractionHandler StatChanged; //event function list to call when a stat has changed

    public string statName;
    public float current;
    public float max;
    public bool canNegative = false;
    float lastCurrent = 0.0f;

    //return the percentage of this stat current / max
    public float percentage()
    {
        if (max > 0)
        {
            return current / max;
        }
        else
            return 0.0f;
    }

    //Stat has changed, if there are subscribers, notify them
    public virtual void OnStatChanged()
    {
        if (StatChanged != null)
        {
            StatChanged(this);
        }
    }

    //If our stat has changed, call StatChanged to let those watching this stat know it has changed
    void Update()
    {
        if(lastCurrent != current)
        {
            //generally stat changed is called when changes occur in this class. If changes occur to our public variable outside of this class, let everyone know.
            lastCurrent = current;
            StatChanged(this);
        }
    }

    //Set current to amount
    public void SetStatCurrent(float amount)
    {
        current = amount;
        Clamp();
        OnStatChanged();
    }

    //Set current to max
    public void SetToMax()
    {
        current = max;
        OnStatChanged();
    }

    //Set current to zero
    public void SetToZero()
    {
        current = 0;
        OnStatChanged();
    }

    //Increment current by amount
    public void IncrementBy(float amount)
    {
        current += amount;
        Clamp();
        OnStatChanged();
    }

    void Clamp()
    {
        //Clamp in range of 0 to max values or if it can be negative just clamp to max and -max

        //upper bound
        if (current > max)
            current = max;

        //lower bound
        if (!canNegative && current < 0)
            current = 0;
        else if(canNegative && current < -max)
            current = -max;
    }

    
}
