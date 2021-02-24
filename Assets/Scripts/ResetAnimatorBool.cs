using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    [System.Serializable]
    public struct BoolStatus
    {
        public string targetBool;
        public bool status;
    }


    //public string targetBool;
    //public bool status;

    /*public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(targetBool, status);
    }*/

    public BoolStatus[] boolStatuses;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        foreach (BoolStatus b in boolStatuses)
        {
            animator.SetBool(b.targetBool, b.status);
        }
    }

}
