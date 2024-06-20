using Zenject;
using UnityEngine;
using System.Collections.Generic;

namespace Match3
{
    public abstract class ChainBase
    {
        public abstract List<GridObject<Gem>> Chain { get; }
        public abstract int Priority { get; protected set; }
        public abstract bool HasChain(GridObject<Gem> gridObject);
        [Inject] private GridSystem<GridObject<Gem>> gridSystem;
        protected ChainBase(GridSystem<GridObject<Gem>> gridSystem, int priority = 0)
        {
            this.gridSystem = gridSystem;
            this.Priority = priority;
        }
        protected List<GridObject<Gem>> FindLinearChain(GridObject<Gem> gridObject, Vector2Int direction, int range)
        {
            List<GridObject<Gem>> chainReference = new();
            for (int i = 1; i < range; i++)
            {
                Vector2Int pos = gridSystem.GetXY(gridObject) + direction * i;
                var targetGridObject = gridSystem.GetValue(pos.x, pos.y);

                if (targetGridObject == null) break;

                if (targetGridObject.GetValue().GemType == gridObject.GetValue().GemType)
                {
                    chainReference.Add(targetGridObject);
                }
                else
                {
                    break;
                }
            }
            return chainReference;
        }
    }
}