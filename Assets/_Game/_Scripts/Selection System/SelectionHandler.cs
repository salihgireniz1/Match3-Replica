using Zenject;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Match3
{
    /// <summary>
    /// Handles the logic for gem selections and processes the selected gems.
    /// </summary>
    public class SelectionHandler : MonoBehaviour
    {
        /// <summary>
        /// Injected instance of GemSelector.
        /// </summary>
        [Inject]
        GemSelector selector;

        /// <summary>
        /// Injected instance of SelectionProvider.
        /// </summary>
        [Inject]
        SelectionProvider selectionProvider;

        [Inject]
        GameStateMachine gameStateMachine;

        private void OnEnable()
        {
            selector.OnGemsSelected += HandleSelections;
        }
        private void OnDisable()
        {
            selector.OnGemsSelected -= HandleSelections;
        }
        /// <summary>
        /// Processes the selected gems and performs the necessary actions.
        /// </summary>
        /// <param name="selected">The selected gems.</param>
        /// <param name="switchable">The switchable gems.</param>
        /// <returns>An async Task representing the asynchronous processing of the selected gems.</returns>
        public async UniTask HandleSelections(GridObject<BaseGem> selected, GridObject<BaseGem> switchable)
        {
            Selection selectionProcess = selectionProvider.GetValidSelectionProcess(selected, switchable);
            if (selectionProcess != null)
            {
                await selectionProcess.Process(selected, switchable);
            }

            gameStateMachine.ChangeState(gameStateMachine.Playing);
        }
        // List<GridObject<BaseGem>> GetChain(GridObject<BaseGem> gridObject)
        // {
        //     List<GridObject<BaseGem>> visited = new List<GridObject<BaseGem>>();
        //     return GetChainableNeighbors(gridObject, visited);
        // }
        // List<GridObject<BaseGem>> GetChainableNeighbors(GridObject<BaseGem> gridObject, List<GridObject<BaseGem>> visited)
        // {
        //     List<GridObject<BaseGem>> objectSpecificChain = new();

        //     if (visited.Contains(gridObject))
        //     {
        //         return objectSpecificChain; // Return an empty list if already visited
        //     }
        //     visited.Add(gridObject); // Mark the current gridObject as visited
        //     var gemNeighbors = grid.GetNeighbors2D(gridObject);
        //     foreach (GridObject<BaseGem> neighbor in gemNeighbors)
        //     {
        //         if (gridObject.GetValue().GemType == neighbor.GetValue().GemType)
        //         {
        //             var neighborChain = GetChainableNeighbors(neighbor, visited);
        //             objectSpecificChain.AddRange(neighborChain);
        //         }
        //     }
        //     objectSpecificChain.Add(gridObject);
        //     return objectSpecificChain;
        // }
    }
}