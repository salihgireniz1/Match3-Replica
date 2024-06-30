using DG.Tweening;
using UnityEngine;
namespace Match3
{
    public abstract class AnimationData : ScriptableObject
    {
        public abstract float Duration { get; protected set; }
        public abstract Ease Ease { get; protected set; }
    }
}