using Cysharp.Threading.Tasks;

namespace Match3
{
    public interface IGridControls
    {
        public GridSystem<GridObject<BaseGem>> Grid { get; }

        void ClearGridObject(GridObject<BaseGem> gridObject);
        void CreateGridObject(int x, int y);
        UniTask ReassignAllGridContents();
        void FillGrid();
    }
}