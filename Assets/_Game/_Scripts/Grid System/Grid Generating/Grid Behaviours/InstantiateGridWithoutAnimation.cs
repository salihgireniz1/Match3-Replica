using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class InstantiateGridWithoutAnimation : IGridControls
    {
        int Width => match3Data.width;
        int Height => match3Data.height;
        [Inject] private GemProvider<BaseGem> gemProvider;
        [Inject] private IGemControls gemController;
        [Inject] private Match3Data match3Data;
        [Inject] private IGridMovement gridMovement;
        [Inject] public GridSystem<GridObject<BaseGem>> Grid { get; private set; }

        public void ClearGridObject(GridObject<BaseGem> gridObject)
        {
            var gem = gridObject.GetValue();
            gemProvider.ReturnGem(gem);
            gridObject.SetValue(null);
        }

        public void CreateGridObject(int x, int y)
        {
            GridObject<BaseGem> gridObject = new GridObject<BaseGem>(Grid, x, y); // Create a gem.

            Grid.SetValue(x, y, gridObject); // Set the gem to the grid.

            BaseGem gem = gemProvider.GetRandomGem(); // Get a random gem prefab.
            gridObject.SetValue(gem); // First, assign it to the created gridObject.

            var tripleChainController = new LinearTripleChain(Grid); // Create a chain controller instance.
            while (tripleChainController.HasChain(gridObject)) // Loop for a unique gem to prevent a chain at the starting of the game.
            {
                gemProvider.ReturnGem(gem);
                gem = gemProvider.GetRandomGem();
                gridObject.SetValue(gem);
            }

            gemController.AssignGemToGrid(gem, gridObject);
        }
        // public UniTask CreateGridObjectAbove()
        // {
        //     for (int x = 0; x < Width; x++)
        //     {
        //         for (int y = 0; y < Height; y++)
        //         {
        //             if (Grid.GetValue(x, y) == null)
        //             {

        //             }
        //         }
        //     }
        // }
        public async UniTask ReassignAllGridContents()
        {
            List<UniTask> assignTasks = new List<UniTask>();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var fallTask = gemController.DropGemContent(Grid.GetValue(x, y));
                    assignTasks.Add(fallTask);
                }
            }
            await UniTask.WhenAll(assignTasks);
        }
        public UniTask AlignContentToGridAnimated(int x, int y)
        {
            var gridToAnimateContent = Grid.GetValue(x, y);
            var gridGem = gridToAnimateContent.GetValue();
            if (gridGem == null)
            {
                Debug.Log("ASDASDA");
                return UniTask.CompletedTask;
            }
            return gridMovement.FallGemMovement(gridGem, Grid.GetWorldPositionCenter(x, y));
        }
        public void FillGrid()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    CreateGridObject(x, y);
                }
            }
        }

    }
}