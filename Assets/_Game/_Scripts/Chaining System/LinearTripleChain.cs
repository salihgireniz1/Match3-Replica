using UnityEngine;
using System.Collections.Generic;

namespace Match3
{
    public class LinearTripleChain : ChainBase
    {
        public override List<GridObject<BaseGem>> Chain => chain;

        public override int Priority { get; protected set; }

        private List<GridObject<BaseGem>> chain = new();

        public LinearTripleChain(GridSystem<GridObject<BaseGem>> gridSystem, int priority = 10) : base(gridSystem, priority) { }

        public override bool HasChain(GridObject<BaseGem> gridObject)
        {
            chain.Clear();
            return HasTripleMatch(gridObject, Vector2Int.right) || HasTripleMatch(gridObject, Vector2Int.up);
        }

        private bool HasTripleMatch(GridObject<BaseGem> gridObject, Vector2Int direction)
        {
            List<GridObject<BaseGem>> currentChain = new List<GridObject<BaseGem>> { gridObject };
            var forwardChain = FindLinearChain(gridObject, direction, 3);
            var backwardChain = FindLinearChain(gridObject, -direction, 3);

            currentChain.AddRange(forwardChain);
            currentChain.AddRange(backwardChain);

            if (currentChain.Count >= 3)
            {
                chain.AddRange(currentChain);
                return true;
            }
            return false;
        }
    }
}