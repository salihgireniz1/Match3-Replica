using UnityEngine;
using Zenject;

namespace Match3
{
    public class Match3 : MonoBehaviour
    {
        [SerializeField] private Match3Data gridData;

        GridGenerator gridGenerator;
        [Inject]
        void Construct(GridGenerator gridGenerator)
        {
            this.gridGenerator = gridGenerator;
        }
        private void Start()
        {
            // Create a grid.
            gridGenerator.FillGrid();
        }
    }
}