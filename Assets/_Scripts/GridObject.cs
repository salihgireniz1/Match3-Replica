namespace Match3
{
    public class GridObject<T>
    {

        GridSystem<GridObject<T>> grid;
        int x, y;

        public GridObject(GridSystem<GridObject<T>> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }
    }
}