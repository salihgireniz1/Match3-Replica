using UnityEngine;

namespace Match3
{
    public class Match3 : MonoBehaviour
    {
        [SerializeField] int width = 8;
        [SerializeField] int height = 8;
        [SerializeField] float cellSize = 1f;
        [SerializeField] Vector3 originPosition = Vector3.zero;
        [SerializeField] bool debug = true;

        GridSystem<GridObject<Gem>> grid;
        private void Start()
        {
            grid = GridSystem<GridObject<Gem>>.VerticalGrid(width, height, cellSize, originPosition, debug);
        }
    }
}