using System.Linq;
using UnityEngine;

namespace Match3
{
    public class InstantiateGem : GemProvider<BaseGem>
    {
        public override BaseGem[] GemPrefabs { get; protected set; }

        public InstantiateGem(Match3Data Match3Data) : base(Match3Data.gemData.gemPrefabs) { }
        public override BaseGem GetGem(GemType gemType)
        {
            return GemPrefabs.FirstOrDefault(g => g.GemType == gemType);
        }

        public override BaseGem GetRandomGem()
        {
            int rnd = UnityEngine.Random.Range(0, GemPrefabs.Length);
            var gem = Object.Instantiate(GemPrefabs[rnd]);
            return gem;
        }

        public override void ReturnGem(BaseGem item)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
}