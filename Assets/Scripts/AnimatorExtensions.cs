using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts
{
    public static class AnimatorExtensions
    {
        public static async UniTask SetTriggerAsync(this Animator animator, int id, CancellationToken cancellationToken = default)
        {
            animator.SetTrigger(id);
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

            try
            {
                await UniTask.WaitForEndOfFrame(cancellationToken);
                while (currentState.normalizedTime < 1)
                {
                    await UniTask.Yield();
                }
            }
            catch (OperationCanceledException)
            {
                animator.Play(currentState.shortNameHash, 0,1);
            }
        }
    }
}