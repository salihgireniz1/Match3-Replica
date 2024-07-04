using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class AnimatedGemController : IGemControls
    {
        private Transform cellParent = new GameObject("Cell Parent").transform;
        [Inject] public GridSystem<GridObject<BaseGem>> Grid { get; private set; }
        [Inject] IGridMovement gridMovement;
        public void AssignGemToGrid(BaseGem gem, GridObject<BaseGem> gridObject)
        {
            if (gem == null) return;
            Vector2Int gridObjIndices = Grid.GetXY(gridObject);
            gem.name = "Gem (" + gridObjIndices.x + "," + gridObjIndices.y + ")"; // Assign a indendifier name with indices.
            gem.transform.SetParent(cellParent); // Collect spawned gems under an object in hierarchy.
            GridObject<BaseGem> gridObjectToAlignInto = Grid.GetValue(gridObjIndices.x, gridObjIndices.y);
            gridObjectToAlignInto.SetValue(gem);
        }

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

            var x = Swipe(selected, switchableIndices);
            var y = Swipe(switchable, selectedIndices);
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
            return gridMovement.SwipedGemMovement(obj, switchablePos);
        }
    }
}