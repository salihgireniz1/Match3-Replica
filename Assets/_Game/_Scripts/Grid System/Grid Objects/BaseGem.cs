using UnityEngine;

namespace Match3
{
    public abstract class BaseGem : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// This property is responsible for providing the type of the gem.
        /// </summary>
        /// <value>
        /// The type of the gem.
        /// </value>
        public abstract GemType GemType { get; }


        public abstract void MergeWith(GridObject<BaseGem> other);
        #endregion
    }
}