using Zenject;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Match3
{
    public class ChainProvider
    {
        public List<ChainBase> ChainTypes { get; private set; }
        public ChainProvider([Inject] List<ChainBase> injectedChains)
        {
            ChainTypes = injectedChains.OrderByDescending(x => x.Priority).ToList();
        }
        public ChainBase GetValidChain(GridObject<BaseGem> gridObject)
        {
            try
            {
                var chainBase = ChainTypes.First(x => x.HasChain(gridObject));
                Debug.Log($"{gridObject.GetValue().name} has {chainBase.GetType()} type chain!", gridObject.GetValue());

                return chainBase;
            }
            catch
            {
                Debug.Log($"{gridObject.GetValue().name} has no chain.", gridObject.GetValue());
                return null;
            }

        }
    }
}