using UnityEngine;
using Zenject;

namespace Match3
{
    public class Match3 : MonoBehaviour
    {
        [SerializeField] private Match3Data gridData;

        [Inject] GridFiller gridFiller;

        private void Start()
        {
            // Create a grid.
            gridFiller.FillGrid();
        }
    }
}