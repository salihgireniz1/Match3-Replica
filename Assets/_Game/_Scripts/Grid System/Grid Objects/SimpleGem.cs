using System;
using UnityEngine;

namespace Match3
{
    public class SimpleGem : BaseGem
    {
        #region Fields
        [SerializeField] private GemType gemType = GemType.Red;

        #endregion
        public override GemType GemType => gemType;


        public override void MergeWith(GridObject<BaseGem> other)
        {

        }
    }
}