using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public GameObject model;
    public CameraController camcon;
    public IUserInput pi;
    public float walkSpeed = 2.4f;
    public float runMultiplier = 2.0f;
    public float JumpVelocity = 3.0f;
    public float rollVelocity = 5.0f;

    [Space(10)]
    [Header("===== Friction Settings =====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    private Animator anim;
    private Rigidbody rd;
    private Vector3 planeVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private bool lockPlaner = false;
    private bool trackDirection = false;
    private CapsuleCollider col;
    private float lerpTarget;
    private Vector3 deltaPos;

	// Use this for initialization
	void Awake () {
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if(input.enabled==true)
            {
                pi = input;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rd = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update () {

        if(pi.lockon)
        {
            camcon.LockUnlock();
        }
        if (camcon.lockState == false)
        {
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), ((pi.run) ? 2.0f : 1.0f), 0.5f));
            anim.SetFloat("right", 0);
        }
        else
        {
            Vector3 localDvec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("forward", localDvec.z * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localDvec.x * ((pi.run) ? 2.0f : 1.0f));
        }
        anim.SetBool("defense", pi.defense);
        if(pi.roll||rd.velocity.magnitude>7f)
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }
        if(pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        if(pi.attack && CheckState("ground") && canAttack)
        {
            anim.SetTrigger("attack");
        }
        if(camcon.lockState==false)
        {
            if (pi.Dmag > 0.1f)
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
            }
            if (lockPlaner == false)
            {
                planeVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
            }
        }
        else
        {
            if(trackDirection==false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planeVec.normalized;
            }
            if (lockPlaner == false)
            {
                planeVec = pi.Dvec * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
            }
        }


	}

    private void FixedUpdate()
    {
        rd.position += deltaPos;
        rd.velocity = new Vector3(planeVec.x, rd.velocity.y, planeVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    private bool CheckState(string stateName,string layerName="Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName); ;
    }

    /// <summary>
    /// Message processing block
    /// </summary>
    public void OnJumpEnter()
    {
        pi.inputEnable = false;
        lockPlaner = true;
        thrustVec = new Vector3(0, JumpVelocity, 0);
        trackDirection = true;
    }

    public void IsGround()
    {
        anim.SetBool("isGround", true);
    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnable = true;
        lockPlaner = false;
        canAttack = true;
        col.material = frictionOne;
        trackDirection = false;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        pi.inputEnable = false;
        lockPlaner = true;
    }

    public void OnRollEnter()
    {
        thrustVec = new Vector3(0, rollVelocity, 0);
        pi.inputEnable = false;
        lockPlaner = true;
        trackDirection = true;
    }

    public void OnJabEnter()
    {
        
        pi.inputEnable = false;
        lockPlaner = true;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity") * 2;
    }

    public void OnAttack1hAEnter()
    {
        pi.inputEnable = false;
        //lockPlaner = true;
        lerpTarget = 1.0f;
    }

    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.3f));
    }

    public void OnAttackIdleEnter()
    {
        pi.inputEnable = true;
        //lockPlaner = false;
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), 0);
        lerpTarget = 0;
    }

    public void OnAttackIdleUpdate()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.3f));
    }

    public void OnUpdateRM(object _deltaPos)
    {
        if(CheckState("attack1hC","attack"))
        {
            deltaPos += (deltaPos + (Vector3)_deltaPos) / 2.0f;
        }
    }
}
