using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : IUserInput
{

    [Header("===== JoyStick Settings =====")]
    public string axisX = "axisX";
    public string axisY = "axisY";
    public string axisJright = "axis4";
    public string axisJup = "axis5";
    public string btnA = "btn0";
    public string btnB = "btn1";
    public string btnC = "btn2";
    public string btnD = "btn3";
    public string btnLB = "btn4";
    public string btnRB = "btn5";
    public string btnJstick = "btn9";

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonC = new MyButton();
    public MyButton buttonD = new MyButton();
    public MyButton buttonLB = new MyButton();
    public MyButton buttonRB = new MyButton();
    public MyButton buttonJstick = new MyButton();


    //[Header("=====  Output signals  =====")]
    //public float Dup;
    //public float Dright;
    //public float Dmag;
    //public Vector3 Dvec;
    //public float Jup;
    //public float Jright;

    ////1.pressing signal
    //public bool run;
    ////2.trigger once signal
    //public bool jump;
    //private bool lastjump;
    //public bool attack;
    //private bool lastattack;
    ////3.double trigger

    //[Header("=====  Others  =====")]

    //public bool inputEnable = true;

    //private float targetDup;
    //private float targetDright;
    //private float velocityDup;
    //private float velocityDright;

    // Update is called once per frame
    void Update()
    {
        buttonA.Tick(Input.GetButton(btnA));
        buttonB.Tick(Input.GetButton(btnB));
        buttonC.Tick(Input.GetButton(btnC));
        buttonD.Tick(Input.GetButton(btnD));
        buttonLB.Tick(Input.GetButton(btnLB));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonJstick.Tick(Input.GetButton(btnJstick));


        Jup = Input.GetAxis(axisJup);
        Jright = Input.GetAxis(axisJright);

        targetDup = Input.GetAxis(axisY);
        targetDright = Input.GetAxis(axisX);

        if (inputEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempAxis = SquareToCircle(new Vector2(Dright, Dup));

        float Dright2 = tempAxis.x;
        float Dup2 = tempAxis.y;

        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
        jump = buttonA.OnPressed && buttonA.IsExtending;
        roll=buttonA.OnReleased&&buttonA.IsDelaying;
        attack = buttonC.OnPressed;
        defense = buttonRB.IsPressing;
        lockon = buttonJstick.OnPressed;
    }
}
