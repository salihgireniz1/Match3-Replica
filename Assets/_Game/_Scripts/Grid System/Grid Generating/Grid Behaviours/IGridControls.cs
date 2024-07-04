using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Match3
{
    public interface IGridControls
    {
        void ClearGridObject(GridObject<BaseGem> gridObject);
        void CreateGridObject(int x, int y);
        void FillGrid();
        void ClearGridObjects(List<GridObject<BaseGem>> gridObjects);
        void FallReassignment(GridObject<BaseGem> gridObject, int fallCount);
        UniTask AlignGridContents(Dictionary<BaseGem, int> objectDelayPair);
        UniTask FallAnimation(BaseGem gem, Vector3 newPos, int delayTime, CancellationTokenSource cts);
        Dictionary<GridObject<BaseGem>, int> GetObjectFallPair(List<GridObject<BaseGem>> blankGridObjects);
        Dictionary<BaseGem, int> GetObjectDelayPair(List<GridObject<BaseGem>> blankGridObjects);
        void InsertToAnimationQueue(Dictionary<BaseGem, int> objectDelayPair, List<BaseGem> newGridObjects);
        List<BaseGem> CreateGemForNullGridObject(List<GridObject<BaseGem>> nullGridObjects);
    }
}