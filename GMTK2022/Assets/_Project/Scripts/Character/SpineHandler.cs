using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineHandler
{
    private string suffix = "";

    SkeletonGraphic _skeletonAnimation;
    Spine.AnimationState _animationState;
    Spine.Skeleton _skeleton;

    public SpineHandler(GameObject skeletonAnimation, string suffix)
    {
        this.suffix = suffix;

        _skeletonAnimation = skeletonAnimation.GetComponent<SkeletonGraphic>();
        Debug.Log(_skeletonAnimation);
        _animationState = _skeletonAnimation.AnimationState;
        _skeleton = _skeletonAnimation.Skeleton;
    }

    internal void PlayAnimation(string animationName, bool loop = false, bool useSuffix = false)
    {
        if (useSuffix)
            animationName += "_" + suffix;

        _animationState.SetAnimation(0, animationName, loop);
    }
}
