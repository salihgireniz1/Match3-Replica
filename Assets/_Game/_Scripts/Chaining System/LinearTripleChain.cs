using UnityEngine;
using System.Collections.Generic;

namespace Match3
{
    public class LinearTripleChain : ChainBase
    {
        public override List<GridObject<BaseGem>> Chain { get; } = new();

        public override int Priority { get; protected set; } = 0;

        public LinearTripleChain(GridSystem<GridObject<BaseGem>> gridSystem, int priority = 0) : base(gridSystem, priority) { }

        public override bool HasChain(GridObject<BaseGem> gridObject)
        {
            Chain?.Clear();
            return HasLinearChain(gridObject, Vector2Int.right, 3) || HasLinearChain(gridObject, Vector2Int.up, 3);
        }


    }
}