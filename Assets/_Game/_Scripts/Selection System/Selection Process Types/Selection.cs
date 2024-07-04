using Zenject;
using Cysharp.Threading.Tasks;

namespace Match3
{
    public abstract class Selection
    {
        [Inject]
        protected ChainProvider ChainProvider { get; }
        public abstract bool IsValid(BaseGem gem1 = null, BaseGem gem2 = null);
        public abstract UniTask Process(
            GridObject<BaseGem> touchedObject = null,
            GridObject<BaseGem> finalObject = null);
        public virtual async UniTask ManageMatch(GridObject<BaseGem> obj)
        {
            // Seek for best chain matching
            ChainBase objectChain = ChainProvider.GetValidChain(obj);
            if (objectChain == null) return;
        }
    }
}