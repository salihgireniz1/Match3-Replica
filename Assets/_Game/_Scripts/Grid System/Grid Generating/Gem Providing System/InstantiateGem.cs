using System.Linq;
using UnityEngine;

namespace Match3
{
    public class InstantiateGem : GemProvider<BaseGem>
    {
        private Transform cellParent = new GameObject("Gems Holder").transform;
        public override BaseGem[] GemPrefabs { get; protected set; }

        public InstantiateGem(Match3Data Match3Data) : base(Match3Data.gemData.gemPrefabs) { }
        public override BaseGem GetGem(GemType gemType)
        {
            return GemPrefabs.FirstOrDefault(g => g.GemType == gemType);
        }

        /// <summary>
        /// Instantiates and returns a random gem from the available gem prefabs.
        /// </summary>
        /// <returns>A randomly instantiated gem from the GemPrefabs array.</returns>
        public override BaseGem GetRandomGem()
        {
            // Generate a random index within the range of GemPrefabs array length.
            int rnd = UnityEngine.Random.Range(0, GemPrefabs.Length);

            // Instantiate a gem at the random index from the GemPrefabs array.
            var gem = Object.Instantiate(GemPrefabs[rnd]);

            // Set the gem's transform parent to the cellParent transform.
            gem.transform.SetParent(cellParent);

            // Return the newly instantiated gem.
            return gem;
        }

        /// <summary>
        /// Destroys the specified gem object and removes it from the game.
        /// </summary>
        /// <param name="item">The gem object to be destroyed and removed from the game.</param>
        public override void ReturnGem(BaseGem item)
        {
            GameObject.Destroy(item.gameObject);
        }

    }
}