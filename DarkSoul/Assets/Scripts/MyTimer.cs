using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer {

    public enum STATE
    {
        IDEL,
        RUN,
        FINISH
    }
    public STATE state;

    public float duration = 1.0f;
    private float elapsedTime = 0;

    public void Tick()
    {
        if(state==STATE.IDEL)
        {

        }
        else if(state==STATE.RUN)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime>=duration)
            {
                state = STATE.FINISH;
            }
        }
        else if(state==STATE.FINISH)
        {

        }
        else
        {
            Debug.Log("MyTimer erron");
        }
    }

    public void Go()
    {
        elapsedTime = 0;
        state = STATE.RUN;
    }
}
