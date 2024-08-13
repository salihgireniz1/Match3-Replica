using Zenject;
using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Match3
{
    public class TweenForSwipe : IGemMovement
    {
        private readonly SwitchAnimationData switchData; // Switch animation data( i.e., duration, ease).
        [Inject]
        public TweenForSwipe(SwitchAnimationData switchData)
        {
            this.switchData = switchData;
        }
        /// <summary>
        /// Aligns gem to position with animation on swipe.
        /// </summary>
        /// <param name="gem">Gem to move.</param>
        /// <param name="movePos">Swiped position.</param>
        /// <returns>Tween animation converted into UniTask.</returns>
        public UniTask MoveGem(BaseGem gem, Vector3 movePos)
        {
            return gem.transform.DOMove(movePos, switchData.Duration).SetEase(switchData.Ease).ToUniTask();
        }
    }
}