namespace Match3
{
    public class GridObject<T>
    {

        GridSystem<GridObject<T>> grid;
        int x, y;
        private T gem;

        public GridObject(GridSystem<GridObject<T>> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetValue(T gem)
        {
            this.gem = gem;
        }
        public T GetValue()
        {
            return this.gem;
        }
    }

}