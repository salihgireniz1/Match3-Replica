using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Match3
{
    public abstract class ChainBase
    {
        public abstract List<GridObject<BaseGem>> Chain { get; }
        public abstract int Priority { get; protected set; }
        public abstract bool HasChain(GridObject<BaseGem> gridObject);
        [Inject] GridSystem<GridObject<BaseGem>> gridSystem;
        [Inject] IGridControls gridBehaviour;
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

                var baseGem = targetGridObject.GetValue();
                if (baseGem == null) break;

                if (baseGem.GemType == gridObject.GetValue().GemType)
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
        protected bool HasLinearChain(GridObject<BaseGem> gridObject, Vector2Int direction, int desiredAmount)
        {
            List<GridObject<BaseGem>> currentChain = new List<GridObject<BaseGem>> { gridObject };
            var forwardChain = FindLinearChain(gridObject, direction, desiredAmount);
            var backwardChain = FindLinearChain(gridObject, -direction, desiredAmount);

            currentChain.AddRange(forwardChain);
            currentChain.AddRange(backwardChain);

            if (currentChain.Count >= desiredAmount)
            {
                Chain.AddRange(currentChain);
                return true;
            }
            return false;
        }
    }
}