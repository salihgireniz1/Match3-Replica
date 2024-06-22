using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Match3
{
    public class InGridMovement
    {
        public GridSystem<GridObject<BaseGem>> Grid => grid;
        public SwitchAnimationData AnimationData => animationData;
        private GridSystem<GridObject<BaseGem>> grid;
        private SwitchAnimationData animationData;
        public InGridMovement(GridSystem<GridObject<BaseGem>> grid, SwitchAnimationData animationData)
        {
            this.grid = grid;
            this.animationData = animationData;
        }

        /// <summary>
        /// Switches two grid objects' positions.
        /// </summary>
        /// <param name="selected">The selected grid object.</param>
        /// <param name="switchable">The switchable grid object.</param>
        /// <returns>A UniTask that completes when both selected and switchable gems have been moved to their new positions.</returns>
        public UniTask SwitchGridObjects(GridObject<BaseGem> selected, GridObject<BaseGem> switchable)
        {
            Vector2Int selectedIndices = grid.GetXY(selected);
            Vector2Int switchableIndices = grid.GetXY(switchable);

            var moveSelected = SwitchGridObject(selected, switchableIndices);
            var moveSwitchable = SwitchGridObject(switchable, selectedIndices);

            // Update GridObjects' gem values
            BaseGem selectedGem = selected.GetValue();
            BaseGem switchableGem = switchable.GetValue();
            selected.SetValue(switchableGem);
            switchable.SetValue(selectedGem);

            return UniTask.WhenAll(moveSelected, moveSwitchable);
        }

        public UniTask SwitchGridObject(GridObject<BaseGem> obj, Vector2Int newIndices)
        {
            Vector3 switchablePos = grid.GetWorldPositionCenter(newIndices.x, newIndices.y);
            return MoveGemToNewPosition(obj, switchablePos);
        }
        /// <summary>
        /// Moves a gem to a new position by animating its transformation.
        /// </summary>
        /// <param name="gemToMove">The grid object containing the gem to be moved.</param>
        /// <param name="movePos">The new position to which the gem should be moved.</param>
        /// <returns>A UniTask that completes when the gem has been moved to its new position.</returns>
        public UniTask MoveGemToNewPosition(GridObject<BaseGem> gemToMove, Vector3 movePos)
        {
            if (animationData == null)
            {
                Debug.Log("Anim Data null");
            }
            return gemToMove.GetValue().transform.DOMove(movePos, animationData.duration).SetEase(animationData.ease).ToUniTask();
        }
    }
}