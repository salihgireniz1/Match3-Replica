using DG.Tweening;
using UnityEngine;
namespace Match3
{
    [CreateAssetMenu(fileName = "Switch Anim Data", menuName = "Scriptables/Switch Animation Data")]
    public class SwitchAnimationData : AnimationData
    {
        [field: SerializeField] public override float Duration { get; protected set; }
        [field: SerializeField] public override Ease Ease { get; protected set; }
    }
}