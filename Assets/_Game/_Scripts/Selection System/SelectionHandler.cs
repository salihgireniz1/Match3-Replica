using Zenject;
using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Match3
{
    public class SelectionHandler : MonoBehaviour
    {
        /// <summary>
        /// The grid system that manages the game board.
        /// </summary>
        [Inject]
        GridSystem<GridObject<BaseGem>> grid;

        [Inject]
        SwitchAnimationData animationData;

        [Inject]
        GemSelector selector;

        [Inject, SerializeField]
        SelectionProvider selectionProvider;

        private void OnEnable()
        {
            selector.OnGemsSelected += HandleSelections;
        }
        private void OnDisable()
        {
            selector.OnGemsSelected -= HandleSelections;
        }
        public async void HandleSelections(GridObject<BaseGem> selected, GridObject<BaseGem> switchable)
        {
            // This is the case we will just manage default functionality
            // of selected gem. For example, if selected gem is a bomb,
            // we will just explode it and manage the rest, like deleting
            // near gems etc.
            if (selected != null && switchable == null)
            {

            }
            // This is the case we will switch two gems.
            else if (selected != null && switchable != null)
            {
                SelectionProcess selectionProcess = selectionProvider.GetValidSelectionProcess(selected, switchable);
                if (selectionProcess != null)
                {
                    await selectionProcess.Process(selected, switchable);
                }
                // selectionProcess.Process(selected, switchable);
                // await SwitchGridObjects(selected, switchable);

                // Update GridObjects' gem values
                // BaseGem selectedGem = selected.GetValue();
                // BaseGem switchableGem = switchable.GetValue();
                // selected.SetValue(switchableGem);
                // switchable.SetValue(selectedGem);
            }

        }
        List<GridObject<BaseGem>> GetChain(GridObject<BaseGem> gridObject)
        {
            List<GridObject<BaseGem>> visited = new List<GridObject<BaseGem>>();
            return GetChainableNeighbors(gridObject, visited);
        }
        List<GridObject<BaseGem>> GetChainableNeighbors(GridObject<BaseGem> gridObject, List<GridObject<BaseGem>> visited)
        {
            List<GridObject<BaseGem>> objectSpecificChain = new();

            if (visited.Contains(gridObject))
            {
                return objectSpecificChain; // Return an empty list if already visited
            }
            visited.Add(gridObject); // Mark the current gridObject as visited
            var gemNeighbors = grid.GetNeighbors2D(gridObject);
            foreach (GridObject<BaseGem> neighbor in gemNeighbors)
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
    }

    [Serializable]
    public class SelectionProvider
    {
        public List<SelectionProcess> SelectionProcesses => selectionProcesses;

        [SerializeField]
        List<SelectionProcess> selectionProcesses = new();


        // Initialize all selection type classes.
        public SelectionProvider(SimpleToSimpleSelection simp2simp)
        {
            selectionProcesses.Add(simp2simp);
        }

        public SelectionProcess GetValidSelectionProcess(GridObject<BaseGem> gridObject1, GridObject<BaseGem> gridObject2)
        {
            return selectionProcesses.First(x => x.IsValid(gridObject1.GetValue(), gridObject2.GetValue()));
        }
    }

}