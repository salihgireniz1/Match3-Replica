using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Match3
{
    public class InstantGemMover : IGemMovement
    {
        /// <summary>
        /// Instantly aligns the gem to the specified position.
        /// </summary>
        /// <returns>Completed task.</returns>
        public UniTask MoveGem(BaseGem gem, Vector3 movePos)
        {
            gem.transform.position = movePos;
            return UniTask.CompletedTask;
        }
    }
}