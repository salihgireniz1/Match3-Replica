using System.Linq;
using UnityEngine;

namespace Match3
{
    public class InstantiateGem : GemProvider<Gem>
    {
        public override Gem[] GemPrefabs { get; protected set; }

        public InstantiateGem(Match3Data Match3Data) : base(Match3Data.gemData.gemPrefabs) { }
        public override Gem GetGem(GemType gemType)
        {
            return GemPrefabs.FirstOrDefault(g => g.GemType == gemType);
        }

        public override Gem GetRandomGem()
        {
            int rnd = UnityEngine.Random.Range(0, GemPrefabs.Length);
            // var gem = MonoBehaviour.Instantiate(GemPrefabs[rnd]);
            return GemPrefabs[rnd];
        }

        public override void ReturnGem(Gem item)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
}