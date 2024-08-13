using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Match3
{
    public abstract class GridFiller
    {
        [Inject] protected Match3Data GridData;
        [Inject] protected GridSystem<GridObject<BaseGem>> Grid;
        [Inject] protected GemProvider<BaseGem> gemProvider;

        /// <summary>
        /// Creates a new grid object at the specified coordinates (x, y) and assigns it to the Grid.
        /// </summary>
        /// <param name="x">The x-coordinate of the grid object.</param>
        /// <param name="y">The y-coordinate of the grid object.</param>
        /// <returns>The newly created grid object.</returns>
        public GridObject<BaseGem> CreateGridObject(int x, int y)
        {
            GridObject<BaseGem> gridObject = new GridObject<BaseGem>(Grid, x, y); // Create a gem.

            Grid.SetValue(x, y, gridObject); // Set the gem to the grid.

            return gridObject;
        }
        /// <summary>
        /// Fills the grid with grid objects.
        /// </summary>
        public abstract void FillGrid();
    }
    public class InstantGridFiller : GridFiller
    {
        LinearTripleChain linearTripleChain;
        [Inject(Id = MoverType.Instant)] private IGemMovement instantMover;
        [Inject] private GridObjectController gridObjectController;

        public override void FillGrid()
        {
            for (int x = 0; x < GridData.width; x++)
            {
                for (int y = 0; y < GridData.height; y++)
                {
                    // Create a new grid object at the specified coordinates (x, y) and assign it to the Grid.
                    var newGridObject = CreateGridObject(x, y);

                    // Get a random, non-matching gem prefab.
                    BaseGem gem = GetUnmatchingGem(newGridObject);

                    // Assign the gem to the grid and position it.
                    gridObjectController.AssignContentToObject(gem, newGridObject);
                    instantMover.MoveGem(gem, Grid.GetWorldPositionCenter(x, y));
                }
            }
        }
        public BaseGem GetUnmatchingGem(GridObject<BaseGem> gridObject)
        {
            BaseGem gem = gemProvider.GetRandomGem(); // Get a random gem prefab.
            gridObject.SetValue(gem); // First, assign it to the created gridObject.
            linearTripleChain = new LinearTripleChain(Grid); // Cache a new chain controller.
            if (linearTripleChain.HasChain(gridObject))
            {
                gemProvider.ReturnGem(gem); // Return the gem with match.
                return GetUnmatchingGem(gridObject); // Recurse into the method till we find a non-match gem.
            }
            return gem; // Return non-matching gem.
        }
    }
    public class GridObjectController
    {
        [Inject] private GridSystem<GridObject<BaseGem>> grid;
        [Inject] private GemProvider<BaseGem> gemProvider;
        public GridObject<BaseGem> CreateGridObject(int x, int y)
        {
            GridObject<BaseGem> gridObject = new GridObject<BaseGem>(grid, x, y); // Create a gem.

            grid.SetValue(x, y, gridObject); // Set the gem to the grid.

            return gridObject;
        }
        public void AssignContentToObject(BaseGem gem, GridObject<BaseGem> gridObject)
        {
            if (gem == null) return;
            Vector2Int gridObjIndices = grid.GetXY(gridObject);
            gem.name = "Gem (" + gridObjIndices.x + "," + gridObjIndices.y + ")"; // Assign a indendifier name with indices.
            gridObject.SetValue(gem);
        }
        public void ClearGridObject(GridObject<BaseGem> gridObject)
        {
            var gem = gridObject.GetValue();
            gemProvider.ReturnGem(gem);
            gridObject.SetValue(null);
        }

    }
    public class InstantiateGridWithoutAnimation : IGridControls
    {
        int Width => match3Data.width;
        int Height => match3Data.height;
        [Inject] private GemProvider<BaseGem> gemProvider;
        [Inject]
        private GridObjectController gridObjectController;
        [Inject] private Match3Data match3Data;
        [Inject] private IGridMovement gemMovement;
        [Inject] private ChainProvider chainProvider;
        [Inject] public GridSystem<GridObject<BaseGem>> Grid { get; private set; }
        [Inject(Id = MoverType.Instant)] private IGemMovement instantMover;
        public List<BaseGem> CreateGemForNullGridObject(List<GridObject<BaseGem>> nullGridObjects)
        {
            Dictionary<int, int> objectXCountPairs = new Dictionary<int, int>();
            List<BaseGem> newGems = new();
            foreach (GridObject<BaseGem> nullGridObject in nullGridObjects)
            {
                var indices = Grid.GetXY(nullGridObject);
                if (!objectXCountPairs.ContainsKey(indices.x)) objectXCountPairs[indices.x] = 0;
                BaseGem newGem = gemProvider.GetRandomGem();
                newGems.Add(newGem);
                Vector3 gemPos = Vector3.up * objectXCountPairs[indices.x] + Grid.GetWorldPositionAboveScreen(indices.x);
                instantMover.MoveGem(newGem, gemPos);
                for (int y = 0; y < Width; y++)
                {
                    var gridObject = Grid.GetValue(indices.x, y);
                    if (gridObject.GetValue() == null)
                    {
                        gridObject.SetValue(newGem);
                        // gemController.AssignGemToGrid(newGem, gridObject);
                        gridObjectController.AssignContentToObject(newGem, gridObject);
                        break;
                    }
                }
                objectXCountPairs[indices.x]++;
            }
            return newGems;
        }
        public void InsertToAnimationQueue(Dictionary<BaseGem, int> objectDelayPair, List<BaseGem> newGridObjects)
        {
            foreach (BaseGem newObject in newGridObjects)
            {
                int fallingObjectBelowCount = 0;
                foreach (var pair in objectDelayPair)
                {
                    if (pair.Key.transform.position.x == newObject.transform.position.x)
                    {
                        fallingObjectBelowCount++;
                    }
                }
                objectDelayPair.Add(newObject, fallingObjectBelowCount);
            }
        }

        public void FallReassignment(GridObject<BaseGem> gridObject, int fallCount)
        {
            BaseGem content = gridObject.GetValue();
            if (content == null) return;
            Vector2Int currentIndices = Grid.GetXY(gridObject);
            Vector2Int newIndices = currentIndices - new Vector2Int(0, fallCount);
            GridObject<BaseGem> gridObjectToAssingInto = Grid.GetValue(newIndices.x, newIndices.y);
            gridObject.SetValue(null);
            gridObjectController.AssignContentToObject(content, gridObjectToAssingInto);
        }
        CancellationTokenSource cts;
        public UniTask AlignGridContents(Dictionary<BaseGem, int> objectDelayPair)
        {
            cts?.Cancel();
            cts = new();
            List<UniTask> result = new List<UniTask>();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var gridObject = Grid.GetValue(x, y);
                    var gem = gridObject.GetValue();
                    if (gem == null) continue;
                    if (!objectDelayPair.ContainsKey(gem)) continue;

                    var gemFeltFrom = Grid.GetXY(gem.transform.position);
                    var feltDistance = gemFeltFrom.y - y;
                    if (feltDistance <= 0) continue;
                    int delay = objectDelayPair[gem];
                    var task = FallAnimation(gem, Grid.GetWorldPositionCenter(x, y), delay, cts);
                    result.Add(task);
                }
            }
            return UniTask.WhenAll(result).AttachExternalCancellation(cts.Token);
        }

        public async UniTask FallAnimation(BaseGem gem, Vector3 newPos, int delayTime, CancellationTokenSource cts)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime * 0.05f));
            await gemMovement.FallGemMovement(gem, newPos, cts);
            var gridIndicesFeltOn = Grid.GetXY(newPos);
            var gridFeltOn = Grid.GetValue(gridIndicesFeltOn.x, gridIndicesFeltOn.y);
            var validChain = chainProvider.GetValidChain(gridFeltOn);
            if (validChain != null) await validChain.ManageChain(this);
        }

        public Dictionary<GridObject<BaseGem>, int> GetObjectFallPair(List<GridObject<BaseGem>> blankGridObjects)
        {
            var result = new Dictionary<GridObject<BaseGem>, int>();
            foreach (GridObject<BaseGem> blank in blankGridObjects)
            {
                Vector2Int index = Grid.GetXY(blank);
                GridObject<BaseGem>[] column = Grid.GetColumnValues(index.x);
                for (int y = index.y + 1; y < Height; y++)
                {
                    if (column[y].GetValue() == null) continue;
                    if (result.ContainsKey(column[y])) continue;
                    result[column[y]] = GetBlankCountBelow(column[y]);
                }
            }
            return result;
        }

        public Dictionary<BaseGem, int> GetObjectDelayPair(List<GridObject<BaseGem>> blankGridObjects)
        {
            var result = new Dictionary<BaseGem, int>();
            foreach (GridObject<BaseGem> blank in blankGridObjects)
            {
                Vector2Int index = Grid.GetXY(blank);
                GridObject<BaseGem>[] column = Grid.GetColumnValues(index.x);
                for (int y = index.y + 1; y < Height; y++)
                {
                    BaseGem gem = column[y].GetValue();
                    if (gem == null) continue;
                    if (result.ContainsKey(gem)) continue;
                    result[gem] = HorizontalDistanceToClosestBlank(column[y]);
                }
            }
            return result;
        }

        int GetBlankCountBelow(GridObject<BaseGem> obj)
        {
            int result = 0;
            Vector2Int indices = Grid.GetXY(obj);
            for (int y = indices.y; y >= 0; y--)
            {
                if (Grid.GetValue(indices.x, y).GetValue() == null)
                {
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// Get distance between grid object and the closest blank grid object at the same column below.
        /// Can be used to calculate animation delay duration.
        /// </summary>
        int HorizontalDistanceToClosestBlank(GridObject<BaseGem> obj)
        {
            if (obj.GetValue() == null) return 0;

            Vector2Int indices = Grid.GetXY(obj);
            for (int y = indices.y; y >= 0; y--)
            {
                if (Grid.GetValue(indices.x, y).GetValue() == null)
                {
                    return indices.y - y;
                }
            }
            return 0;
        }
    }
}