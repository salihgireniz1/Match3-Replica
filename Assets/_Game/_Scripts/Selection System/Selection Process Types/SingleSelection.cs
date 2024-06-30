using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Match3
{
    public class SingleSelection : Selection
    {
        IHaveMonoFunctionality monoFunctialGem;

        /// <summary>
        /// Check if there is a gem clicked on but not swiped it onto something else.
        /// </summary>
        public override bool IsValid(BaseGem gem1 = null, BaseGem gem2 = null)
        {
            bool valid = gem1 != null && gem2 == null;
            if (valid)
            {
                monoFunctialGem = gem1 as IHaveMonoFunctionality;
            }
            return valid;
        }

        public async override UniTask Process(GridObject<BaseGem> touchedObject = null, GridObject<BaseGem> finalObject = null)
        {
            if (monoFunctialGem != null)
            {
                await monoFunctialGem.MonoFunction();
            }
        }
    }
}