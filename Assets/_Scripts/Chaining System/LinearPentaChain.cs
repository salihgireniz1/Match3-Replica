using UnityEngine;
using System.Collections.Generic;

namespace Match3
{
    public class LinearPentaChain : ChainBase
    {
        public override List<GridObject<Gem>> Chain => chain;

        public override int Priority { get; protected set; }

        private List<GridObject<Gem>> chain = new();

        public LinearPentaChain(GridSystem<GridObject<Gem>> gridSystem, int priority = 0) : base(gridSystem, priority) { }

        public override bool HasChain(GridObject<Gem> gridObject)
        {
            chain.Clear();
            return HasPentaMatch(gridObject, Vector2Int.right) || HasPentaMatch(gridObject, Vector2Int.up);
        }

        private bool HasPentaMatch(GridObject<Gem> gridObject, Vector2Int direction)
        {
            List<GridObject<Gem>> currentChain = new List<GridObject<Gem>> { gridObject };
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