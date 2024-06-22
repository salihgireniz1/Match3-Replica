using DG.Tweening;
using UnityEngine;
namespace Match3
{
    [CreateAssetMenu(fileName = "Switch Anim Data", menuName = "Scriptables/SwitchAnimData")]
    public class SwitchAnimationData : ScriptableObject
    {
        public float duration = 0.2f;
        public Ease ease = Ease.Linear;
    }
}