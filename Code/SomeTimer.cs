using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeTimer
{
    private float currentTimePassed;
    public float triggerTime;

    public SomeTimer(float triggerTime)
    {
        this.triggerTime = triggerTime;
        currentTimePassed = 0;
    }

    public void RecordTime(float time)
    {
        currentTimePassed += time;
    }

    public bool HasTimeElapsedTrigger()
    {
        var hasTriggerd = false;
        if( currentTimePassed > triggerTime )
        {
            currentTimePassed = 0;
            return hasTriggerd = true;
        }
        return hasTriggerd;
    }

    public void LowerTriggerTime(int triggerLevel)
    {
    //    if(triggerLevel > 10)
    //    {
    //        triggerTime = 0.25f;
    //    }
    //    else if(triggerLevel > 5)
    //    {
    //        triggerTime = 0.5f;
    //    }
    //    else if(triggerLevel > 3)
    //    {
    //        triggerTime = 1f;
    //    }
    //    else if(triggerLevel > 2)
    //    {
    //        triggerTime = 2f;
    //    }
    //    else if(triggerLevel > 1)
    //    {
    //        triggerTime = 3f;
    //    }
        triggerTime -= triggerLevel;
        if (triggerTime < 1f)
        {
            triggerTime = 1f;
        }
    }

}