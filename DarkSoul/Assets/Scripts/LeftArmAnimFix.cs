using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour {

    private Animator anim;

    public Vector3 A;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if (anim.GetBool("defense") == false)
        {
            Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            leftLowerArm.localEulerAngles += A;
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));
        }
    }
}
