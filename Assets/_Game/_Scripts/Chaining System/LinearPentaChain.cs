using UnityEngine;
using System.Collections.Generic;

namespace Match3
{
    public class LinearPentaChain : ChainBase
    {
        public override List<GridObject<BaseGem>> Chain => chain;

        public override int Priority { get; protected set; }

        private List<GridObject<BaseGem>> chain = new();

        public LinearPentaChain(GridSystem<GridObject<BaseGem>> gridSystem, int priority = 0) : base(gridSystem, priority) { }

        public override bool HasChain(GridObject<BaseGem> gridObject)
        {
            chain.Clear();
            return HasPentaMatch(gridObject, Vector2Int.right) || HasPentaMatch(gridObject, Vector2Int.up);
        }

        private bool HasPentaMatch(GridObject<BaseGem> gridObject, Vector2Int direction)
        {
            List<GridObject<BaseGem>> currentChain = new List<GridObject<BaseGem>> { gridObject };
            var forwardChain = FindLinearChain(gridObject, direction, 5);
            var backwardChain = FindLinearChain(gridObject, -direction, 5);

            currentChain.AddRange(forwardChain);
            currentChain.AddRange(backwardChain);

            if (currentChain.Count >= 5)
            {
                chain.AddRange(currentChain);
                return true;
            }
            return false;
        }
    }
}