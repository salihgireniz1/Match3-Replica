using UnityEngine;
using Zenject;

namespace Match3
{
    public class GridGenerator
    {
        #region Fields
        [SerializeField] int width = 8;
        [SerializeField] int height = 8;
        [SerializeField] float cellSize = 1f;
        [SerializeField] Vector3 originPosition = Vector3.zero;
        [SerializeField] bool debug = true;
        [SerializeField] private Transform cellParent;
        [Inject] GridSystem<GridObject<Gem>> grid;
        GemProvider<Gem> gemProvider;
        #endregion

        #region Constructor
        public GridGenerator(Match3Data gridData, GemProvider<Gem> gemProvider)
        {
            width = gridData.width;
            height = gridData.height;
            cellSize = gridData.cellSize;
            originPosition = gridData.originPosition;
            debug = gridData.debug;
            this.gemProvider = gemProvider;
            cellParent = new GameObject("Cell Parent").transform;
        }
        #endregion

        #region Methods

        public void FillGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CreateGem(x, y);
                }
            }
        }

        private void CreateGem(int x, int y)
        {
            GridObject<Gem> gridObject = new GridObject<Gem>(grid, x, y); // Create a gem.

            grid.SetValue(x, y, gridObject); // Set the gem to the grid.

            Gem gem = gemProvider.GetRandomGem(); // Get a random gem prefab.
            gridObject.SetValue(gem); // First, assign it to the created gridObject.

            var tripleChainController = new LinearTripleChain(grid); // Create a chain controller instance.
            while (tripleChainController.HasChain(gridObject)) // Loop for a unique gem to prevent a chain at the starting of the game.
            {
                gem = gemProvider.GetRandomGem();
                gridObject.SetValue(gem);
            }

            gem = Object.Instantiate(gem); // Instantiate that unique prefab.
            gem.name = "Gem (" + x + "," + y + ")"; // Assign a indendifier name with indices.
            gem.transform.position = grid.GetWorldPositionCenter(x, y); // Align the object to it's gridObject position.
            gem.transform.SetParent(cellParent); // Collect spawned gems under an object in hierarchy.

            gridObject.SetValue(gem); // Finally, re-initialize the gridObject with manipulated gem.
        }
        #endregion
    }
}