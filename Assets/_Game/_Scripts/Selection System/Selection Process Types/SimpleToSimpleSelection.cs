using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
            // Debug.Log($"Now, we will swipe {touchedObject?.GetValue().name} and {finalObject?.GetValue().name} positions.");
            await gemController.SwitchGems(touchedObject, finalObject);
            var destroyedGridObjects = new List<GridObject<BaseGem>>();
            ChainBase touchedGemChain = ChainProvider.GetValidChain(touchedObject);
            if (touchedGemChain != null)
            {
                // Debug.Log($"<----- Destroying {touchedObject?.GetValue().name}'s chain ----->");
                destroyedGridObjects.AddRange(touchedGemChain.Chain);
                _ = touchedGemChain.ManageChain(gridController);
            }

            ChainBase finalGemChain = ChainProvider.GetValidChain(finalObject);
            if (finalGemChain != null)
            {
                // Debug.Log($"<----- Destroying {finalObject?.GetValue()?.name}'s chain ----->");
                destroyedGridObjects.AddRange(finalGemChain.Chain);
                _ = finalGemChain.ManageChain(gridController);
            }

            // destroyedGridObjects = destroyedGridObjects.Distinct().ToList(); // Clear duplicates if any.
            // gridController.ClearGridObjects(destroyedGridObjects);

            // Dictionary<GridObject<BaseGem>, int> objectFallPair = gridController.GetObjectFallPair(destroyedGridObjects);
            // Dictionary<BaseGem, int> objectDelayPair = gridController.GetObjectDelayPair(destroyedGridObjects);

            // foreach (var pair in objectFallPair)
            // {
            //     gridController.FallReassignment(pair.Key, pair.Value);
            // }
            // var newGems = gridController.CreateGemForNullGridObject(destroyedGridObjects);
            // gridController.InsertToAnimationQueue(objectDelayPair, newGems);
            // _ = gridController.AlignGridContents(objectDelayPair);
        }
    }
}