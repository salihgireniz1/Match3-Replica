using DG.Tweening;
using UnityEngine;
namespace Match3
{
    [CreateAssetMenu(fileName = "Fall Anim Data", menuName = "Scriptables/Fall Animation Data")]
    public class FallAnimationData : AnimationData
    {
        [field: SerializeField] public override float Duration { get; protected set; }
        [field: SerializeField] public override Ease Ease { get; protected set; }
    }
}