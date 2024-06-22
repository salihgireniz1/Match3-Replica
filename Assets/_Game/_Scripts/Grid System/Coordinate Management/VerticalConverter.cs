using UnityEngine;

namespace Match3
{
    public class VerticalConverter : CoordinateConverter
    {
        public override Vector3 Forward => Vector3.forward;

        public override Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin)
        {
            return new Vector3(x, y, 0) * cellSize + origin;
        }

        public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin)
        {
            return new Vector3((x + 0.5f) * cellSize, (y + 0.5f) * cellSize, 0) + origin;
        }

        public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin)
        {
            Vector3 gridPos = (worldPosition - origin) / cellSize;
            int x = Mathf.FloorToInt(gridPos.x);
            int y = Mathf.FloorToInt(gridPos.y);
            return new Vector2Int(x, y);
        }
    }
}