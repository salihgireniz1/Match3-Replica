using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Match3
{
    public interface IGemMovement
    {
        UniTask MoveGem(BaseGem gem, Vector3 movePos);
    }
    public enum MoverType
    {
        Instant = 0,
        Swipe = 1
    }
}