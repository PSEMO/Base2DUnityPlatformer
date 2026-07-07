using System;
using TMPro;
using UnityEngine;

namespace PSEMO.UI
{
    [Serializable]
    public struct TransitionTextCouple
    {
        public UITransitionPlayer transitionPlayer;
        public TextMeshProUGUI text;
        public TransitionSO transitionData;
        public CanvasGroup canvasGroup;
        public RectTransform rectTransform;
    }
}