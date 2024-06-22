using Zenject;
using UnityEngine;
using System.Collections.Generic;

namespace Match3
{
    public abstract class ChainBase
    {
        public abstract List<GridObject<BaseGem>> Chain { get; }
        public abstract int Priority { get; protected set; }
        public abstract bool HasChain(GridObject<BaseGem> gridObject);
        [Inject] private GridSystem<GridObject<BaseGem>> gridSystem;
        protected ChainBase(GridSystem<GridObject<BaseGem>> gridSystem, int priority = 0)
        {
            this.gridSystem = gridSystem;
            this.Priority = priority;
        }
        protected List<GridObject<BaseGem>> FindLinearChain(GridObject<BaseGem> gridObject, Vector2Int direction, int range)
        {
            List<GridObject<BaseGem>> chainReference = new();
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