using UnityEngine;

namespace Match3
{
    public class Gem : MonoBehaviour, IGridObject
    {
        #region Fields
        [SerializeField] GemType gemType = GemType.Red;
        #endregion

        #region Properties
        public GemType GemType { get => gemType; }

        public void SingleTriggerFunctionality()
        {

        }
        #endregion

        #region Methods

        #endregion
    }
}