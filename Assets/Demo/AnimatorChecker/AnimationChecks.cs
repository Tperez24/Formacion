using System;
using System.Collections;
using UnityEngine;

namespace Demo.AnimatorChecker
{
    public static class AnimationChecks
    {
        public static IEnumerator CheckForAnimationFinished(Animator animator, Action onAnimationFinished)
        {
            var currentAnimation = animator.GetCurrentAnimatorClipInfo(0);
            do
            {
                yield return null;
                
            } while (currentAnimation[0].clip == animator.GetCurrentAnimatorClipInfo(0)[0].clip);

            onAnimationFinished.Invoke();
        }
    }
}