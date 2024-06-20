using UnityEngine;
using System.Collections.Generic;

namespace Match3
{
    public class LinearTripleChain : ChainBase
    {
        public override List<GridObject<Gem>> Chain => chain;

        public override int Priority { get; protected set; }

        private List<GridObject<Gem>> chain = new();

        public LinearTripleChain(GridSystem<GridObject<Gem>> gridSystem, int priority = 10) : base(gridSystem, priority) { }

        public override bool HasChain(GridObject<Gem> gridObject)
        {
            chain.Clear();
            return HasTripleMatch(gridObject, Vector2Int.right) || HasTripleMatch(gridObject, Vector2Int.up);
        }

        private bool HasTripleMatch(GridObject<Gem> gridObject, Vector2Int direction)
        {
            List<GridObject<Gem>> currentChain = new List<GridObject<Gem>> { gridObject };
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