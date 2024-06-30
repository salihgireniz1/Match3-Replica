using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Match3
{
    public interface IGemControls
    {
        int GetFallUnitCount(int x, int y);
        UniTask DropGemContent(GridObject<BaseGem> gridObj);
        UniTask AssignGemToIndex(BaseGem gem, Vector2Int newIndices);
        void AssignGemToGrid(BaseGem gem, GridObject<BaseGem> gridObject);
        UniTask SwitchGems(GridObject<BaseGem> selected, GridObject<BaseGem> switchable);
    }
}