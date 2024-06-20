using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "Match3 Grid Data", menuName = "Scriptables/Match3 Grid Data")]
    public class Match3Data : ScriptableObject
    {
        public int width = 8;
        public int height = 8;
        public float cellSize = 1f;
        public Vector3 originPosition = Vector3.zero;
        public bool debug = true;
        public GemData gemData;
    }
}