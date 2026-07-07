using System;
using TMPro;
using UnityEngine;

namespace PSEMO.UI
{
    [Serializable]
    public struct TransitionTextCouple
    {
        public UITransitionPlayer transitionPlayer;
        public TextMeshProUGUI tmp;
        public TransitionSO transitionData;
        public CanvasGroup canvasGroup;
        public RectTransform rectTransform;
    }
}