using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class SwipeWithTween : ISwipeGems
    {
        [Inject(Id = MoverType.Swipe)] IGemMovement swipeTween; // Inject IGemMovement for swipe movement of a gem.
        [Inject] public GridSystem<GridObject<BaseGem>> Grid { get; private set; }

        /// <summary>
        /// Switches two grid objects' positions.
        /// </summary>
        /// <param name="selected">The selected grid object.</param>
        /// <param name="switchable">The switchable grid object.</param>
        /// <returns>A UniTask that completes when both selected and switchable gems have been moved to their new positions.</returns>
        public UniTask SwitchGems(GridObject<BaseGem> selected, GridObject<BaseGem> switchable)
        {
            Vector2Int selectedIndices = Grid.GetXY(selected);
            Vector2Int switchableIndices = Grid.GetXY(switchable);

            var x = Swipe(selected, switchableIndices); // Carry FIRST gem to the place of SECOND one.
            var y = Swipe(switchable, selectedIndices); // Carry SECOND gem to the place of FIRST one.

            // Update GridObjects' gem values
            BaseGem selectedGem = selected.GetValue();
            BaseGem switchableGem = switchable.GetValue();
            selected.SetValue(switchableGem);
            switchable.SetValue(selectedGem);
            return UniTask.WhenAll(x, y);
        }
        public UniTask Swipe(GridObject<BaseGem> obj, Vector2Int newIndices)
        {
            Vector3 switchablePos = Grid.GetWorldPositionCenter(newIndices.x, newIndices.y);
            return swipeTween.MoveGem(obj.GetValue(), switchablePos);
        }
    }
}