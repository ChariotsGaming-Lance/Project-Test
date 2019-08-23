using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimation : StateMachineBehaviour
{
    public string animationName = "";
    public float speed = 1f;
    public bool loop = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SkeletonAnimation animation = animator.GetComponent<SkeletonAnimation>();
        animation.state.SetAnimation(0, animationName, loop).timeScale = speed;
    }
}
