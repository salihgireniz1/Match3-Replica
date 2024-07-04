using Cysharp.Threading.Tasks;

namespace Match3
{
    public interface IGemControls
    {
        void AssignGemToGrid(BaseGem gem, GridObject<BaseGem> gridObject);
        UniTask SwitchGems(GridObject<BaseGem> selected, GridObject<BaseGem> switchable);

    }
}