using Zenject;
using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Match3
{
    public class AnimatedMover : IGridMovement
    {
        [Inject] private SwitchAnimationData switchData;
        [Inject] private FallAnimationData fallData;

        /// <summary>
        /// Moves a gem to a new position by animating its transformation.
        /// </summary>
        /// <param name="gemToMove">The grid object containing the gem to be moved.</param>
        /// <param name="movePos">The new position to which the gem should be moved.</param>
        /// <returns>A UniTask that completes when the gem has been moved to its new position.</returns>
        public UniTask SwipedGemMovement(GridObject<BaseGem> gemToMove, Vector3 movePos)
        {
            return gemToMove.GetValue().transform.DOMove(movePos, switchData.Duration).SetEase(switchData.Ease).ToUniTask();
        }

        public async UniTask FallGemMovement(BaseGem gem, Vector3 newPosition, CancellationTokenSource cts)
        {
            var fallDistance = Vector3.Distance(gem.transform.position, newPosition);
            await gem.transform.DOMoveY(newPosition.y, fallData.Duration * fallDistance).SetEase(fallData.Ease).ToUniTask().AttachExternalCancellation(cts.Token);
        }
    }
    public interface IGridMovement
    {
        UniTask SwipedGemMovement(GridObject<BaseGem> gemToMove, Vector3 movePos);
        UniTask FallGemMovement(BaseGem gem, Vector3 newPosition, CancellationTokenSource cts);
    }
}