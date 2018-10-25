using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IUserInput {

    //Variable
    [Header("=====  Key settings  =====")]
    public string keyUp;
    public string keyDown;
    public string keyRight;
    public string keyLeft;

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    public string keyJright;
    public string keyJleft;
    public string keyJup;
    public string keyJdown;

    [Header("===== Mouse Settings =====")]
    public bool mouseEnable = false;
    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (mouseEnable == true)
        {
            Jup = Input.GetAxis("Mouse Y") * mouseSensitivityX;
            Jright = Input.GetAxis("Mouse X") * mouseSensitivityY;
        }
        else
        {
            Jup = (Input.GetKey(keyJup) ? 1.0f : 0) - (Input.GetKey(keyJdown) ? 1.0f : 0);
            Jright = (Input.GetKey(keyJright) ? 1.0f : 0) - (Input.GetKey(keyJleft) ? 1.0f : 0);
        }

        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if(inputEnable==false)
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

        run = Input.GetKey(keyA);
        defense = Input.GetKey(keyD);

        bool newJump = Input.GetKey(keyB);
        if (newJump != lastjump && newJump == true)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastjump = newJump;

        bool newattack = Input.GetKey(keyC);
        if (newattack != lastattack && newattack == true)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }
        lastattack = newattack;

    }
}
