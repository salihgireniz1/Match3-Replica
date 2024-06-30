using Cysharp.Threading.Tasks;
using Sirenix.Reflection.Editor;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class SimpleToSimpleSelection : Selection
    {
        [Inject] IGridControls gridController;
        [Inject] IGemControls gemController;

        public override bool IsValid(BaseGem gem1, BaseGem gem2)
        {
            return gem1 is SimpleGem && gem2 is SimpleGem;
        }

        public async override UniTask Process(GridObject<BaseGem> touchedObject = null, GridObject<BaseGem> finalObject = null)
        {
            Debug.Log($"Now, we will swipe {touchedObject?.GetValue().name} and {finalObject?.GetValue().name} positions.");
            await gemController.SwitchGems(touchedObject, finalObject);

            ChainBase touchedGemChain = ChainProvider.GetValidChain(touchedObject);
            if (touchedGemChain != null)
            {
                Debug.Log($"<----- Destroying {touchedObject?.GetValue().name}'s chain ----->");
                await touchedGemChain.DestroyChainObjects();
            }

            ChainBase finalGemChain = ChainProvider.GetValidChain(finalObject);
            if (finalGemChain != null)
            {
                Debug.Log($"<----- Destroying {finalObject?.GetValue()?.name}'s chain ----->");
                await finalGemChain.DestroyChainObjects();
            }
            await gridController.ReassignAllGridContents();
        }
    }
}