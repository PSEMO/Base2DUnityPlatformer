using UnityEngine;

namespace PSEMO.UI
{
    [CreateAssetMenu(fileName = "TransitionData", menuName = "SO/UITransition")]
    public class TransitionSO : ScriptableObject
    {
        [Header("Transition Types")]
        public TransitionType transitionType = TransitionType.Fade;

        [Header("Timing")]
        public float duration = 0.25f;
        public bool useSmoothing = true;

        [Header("Slide Settings")]
        public SlideDirection slideDirection = SlideDirection.Down;
        [Tooltip("How far off-screen to slide (in normalized viewport units, 1 = full screen width/height)")]
        public float slideDistance = 1f;

        [Header("Scale Settings")]
        [Tooltip("Scale value when hidden (shown state is always Vector3.one)")]
        public float hiddenScale = 0.3f;

        [Header("Fade Settings")]
        [Tooltip("Fade value when hidden (shown state is always 1.0)")]
        public float hiddenAlpha = 0.4f;
    }
}