using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "Gems", menuName = "Scriptables/Create Gem Data")]
    public class GemData : ScriptableObject
    {
        public Gem[] gemPrefabs;
    }
}
