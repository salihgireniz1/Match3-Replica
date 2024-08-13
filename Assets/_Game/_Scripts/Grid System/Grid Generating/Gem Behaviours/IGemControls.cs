using Cysharp.Threading.Tasks;

namespace Match3
{
    public interface ISwipeGems
    {
        UniTask SwitchGems(GridObject<BaseGem> selected, GridObject<BaseGem> switchable);

    }
}