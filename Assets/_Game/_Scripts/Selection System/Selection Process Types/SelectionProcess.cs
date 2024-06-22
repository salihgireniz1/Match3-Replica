using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Match3
{
    [Serializable]
    public abstract class SelectionProcess
    {
        public abstract bool IsValid(BaseGem gem1, BaseGem gem2);
        public abstract UniTask Process(GridObject<BaseGem> touchedObject = null, GridObject<BaseGem> finalObject = null);

        /// <summary>
        /// Moves a gem to a new position by animating its transformation.
        /// </summary>
        /// <param name="gemToMove">The grid object containing the gem to be moved.</param>
        /// <param name="movePos">The new position to which the gem should be moved.</param>
        /// <returns>A UniTask that completes when the gem has been moved to its new position.</returns>
        // protected UniTask MoveGemToNewPosition(GridObject<BaseGem> gemToMove, Vector3 movePos)
        // {
        //     return gemToMove.GetValue().transform.DOMove(movePos, switchAnimationData.duration).SetEase(switchAnimationData.ease).ToUniTask();
        // }
    }
    public class SimpleToSimpleSelection : SelectionProcess
    {
        [Inject]
        InGridMovement inGridMovement;

        public override bool IsValid(BaseGem gem1, BaseGem gem2)
        {
            return gem1 is SimpleGem && gem2 is SimpleGem;
        }

        public async override UniTask Process(GridObject<BaseGem> touchedObject = null, GridObject<BaseGem> finalObject = null)
        {
            Debug.Log($"Now, we will swipe {touchedObject?.GetValue().name} and {finalObject?.GetValue().name} positions.");
            await inGridMovement.SwitchGridObjects(touchedObject, finalObject);

            // Seek for best chain matching
        }
    }
}