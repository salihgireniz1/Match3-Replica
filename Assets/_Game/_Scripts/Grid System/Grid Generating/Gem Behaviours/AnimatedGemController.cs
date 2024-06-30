using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class AnimatedGemController : IGemControls
    {
        private Transform cellParent = new GameObject("Cell Parent").transform;
        [Inject] public GridSystem<GridObject<BaseGem>> Grid { get; private set; }
        [Inject] IGridMovement gridMovement;
        public UniTask AssignGemToIndex(BaseGem gem, Vector2Int newIndices)
        {
            gem.name = "Gem (" + newIndices.x + "," + newIndices.y + ")"; // Assign a indendifier name with indices.
            gem.transform.SetParent(cellParent); // Collect spawned gems under an object in hierarchy.
            GridObject<BaseGem> gridObjectToAlignInto = Grid.GetValue(newIndices.x, newIndices.y);
            gridObjectToAlignInto.SetValue(gem);
            var falling = gridMovement.FallGemMovement(gem, Grid.GetWorldPositionCenter(newIndices.x, newIndices.y));
            return falling;
        }

        public void AssignGemToGrid(BaseGem gem, GridObject<BaseGem> gridObject)
        {
            Vector2Int gridObjIndices = Grid.GetXY(gridObject);
            gem.name = "Gem (" + gridObjIndices.x + "," + gridObjIndices.y + ")"; // Assign a indendifier name with indices.
            gem.transform.SetParent(cellParent); // Collect spawned gems under an object in hierarchy.
            gem.transform.position = Grid.GetWorldPositionCenter(gridObjIndices.x, gridObjIndices.y); // Align the object to it's gridObject position.
            GridObject<BaseGem> gridObjectToAlignInto = Grid.GetValue(gridObjIndices.x, gridObjIndices.y);
            gridObjectToAlignInto.SetValue(gem);
        }

        public UniTask DropGemContent(GridObject<BaseGem> gridObj)
        {
            var gem = gridObj.GetValue(); // Get the gem to drop down.
            if (gem == null) return UniTask.CompletedTask; // Return if there is nothing to drop.

            Vector2Int indices = Grid.GetXY(gridObj); // Find the indices.
            int fallCount = GetFallUnitCount(indices.x, indices.y); // Find the fall unit count.
            if (fallCount == 0) return UniTask.CompletedTask; // Return if there are no blank grid object below.

            Vector2Int newIndices = new Vector2Int(indices.x, indices.y - fallCount); // Get the new indices of blank grid object at the lowest.
            Debug.Log(gem.name + " will fall for " + fallCount + " units.");
            gridObj.SetValue(null); // Empty the gridObject felt from.

            return AssignGemToIndex(gem, newIndices);
        }

        public int GetFallUnitCount(int x, int y)
        {
            int nullBelowCount = 0;
            var columnMembers = Grid.GetColumnValues(x);
            foreach (var item in columnMembers)
            {
                var itemIndices = Grid.GetXY(item);
                if (itemIndices.y > y) continue;
                else if (item.GetValue() == null) nullBelowCount++;
            }
            return nullBelowCount;
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