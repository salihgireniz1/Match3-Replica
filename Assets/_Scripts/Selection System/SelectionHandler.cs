using Zenject;
using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Match3
{
    public class SelectionHandler : MonoBehaviour
    {
        /// <summary>
        /// The grid system that manages the game board.
        /// </summary>
        [Inject]
        GridSystem<GridObject<Gem>> grid;

        [Inject]
        SwitchAnimationData animationData;

        [Inject]
        GemSelector selector;

        private void OnEnable()
        {
            selector.OnGemsSelected += HandleSelections;
        }
        private void OnDisable()
        {
            selector.OnGemsSelected -= HandleSelections;
        }
        public async void HandleSelections(GridObject<Gem> selected, GridObject<Gem> switchable)
        {
            // This is the case we will just manage default functionality
            // of selected gem. For example, if selected gem is a bomb,
            // we will just explode it and manage the rest, like deleting
            // near gems etc.
            if (selected != null && switchable == null)
            {

                Debug.Log("asdasd");
            }
            // This is the case we will switch two gems.
            else if (selected != null && switchable != null)
            {
                Debug.Log(selected.GetValue().name + "-" + switchable.GetValue().name);
                await SwitchGridObjects(selected, switchable);

                // Update GridObjects' gem values
                Gem selectedGem = selected.GetValue();
                Gem switchableGem = switchable.GetValue();
                selected.SetValue(switchableGem);
                switchable.SetValue(selectedGem);

                // TODO: 
                //If any chain available, handle chain process. Otherwise switch items back.

                // var chain = GetChain(selected);
                // Debug.Log($"Chain count of {selected.GetValue().gameObject.name} is {chain.Count}", selectedGem.gameObject);
            }

        }
        List<GridObject<Gem>> GetChain(GridObject<Gem> gridObject)
        {
            List<GridObject<Gem>> visited = new List<GridObject<Gem>>();
            return GetChainableNeighbors(gridObject, visited);
        }
        List<GridObject<Gem>> GetChainableNeighbors(GridObject<Gem> gridObject, List<GridObject<Gem>> visited)
        {
            List<GridObject<Gem>> objectSpecificChain = new();

            if (visited.Contains(gridObject))
            {
                return objectSpecificChain; // Return an empty list if already visited
            }
            visited.Add(gridObject); // Mark the current gridObject as visited
            var gemNeighbors = grid.GetNeighbors2D(gridObject);
            foreach (GridObject<Gem> neighbor in gemNeighbors)
            {
                if (gridObject.GetValue().GemType == neighbor.GetValue().GemType)
                {
                    var neighborChain = GetChainableNeighbors(neighbor, visited);
                    objectSpecificChain.AddRange(neighborChain);
                }
            }
            objectSpecificChain.Add(gridObject);
            return objectSpecificChain;
        }

        /// <summary>
        /// Switches two grid objects' positions.
        /// </summary>
        /// <param name="selected">The selected grid object.</param>
        /// <param name="switchable">The switchable grid object.</param>
        /// <returns>A UniTask that completes when both selected and switchable gems have been moved to their new positions.</returns>
        UniTask SwitchGridObjects(GridObject<Gem> selected, GridObject<Gem> switchable)
        {
            Vector2Int selectedIndices = grid.GetXY(selected);
            Vector2Int switchableIndices = grid.GetXY(switchable);

            Vector3 selectedPos = grid.GetWorldPositionCenter(selectedIndices.x, selectedIndices.y);
            Vector3 switchablePos = grid.GetWorldPositionCenter(switchableIndices.x, switchableIndices.y);

            var moveSelected = MoveGemToNewPosition(selected, switchablePos);
            var moveSwitchable = MoveGemToNewPosition(switchable, selectedPos);
            return UniTask.WhenAll(moveSelected, moveSwitchable);
        }
        /// <summary>
        /// Moves a gem to a new position by animating its transformation.
        /// </summary>
        /// <param name="gemToMove">The grid object containing the gem to be moved.</param>
        /// <param name="movePos">The new position to which the gem should be moved.</param>
        /// <returns>A UniTask that completes when the gem has been moved to its new position.</returns>
        UniTask MoveGemToNewPosition(GridObject<Gem> gemToMove, Vector3 movePos)
        {
            return gemToMove.GetValue().transform.DOMove(movePos, animationData.duration).SetEase(animationData.ease).ToUniTask();
        }
    }

}