using System.Collections;
using UnityEngine;

namespace PSEMO.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Panel : MonoBehaviour
    {
        public PanelType Type;
        [Header("Animation Settings")]
        [SerializeField] private float fadeDuration = 0.25f;

        [HideInInspector] public bool IsOpen { get; private set; } = false;

        private CanvasGroup _canvasGroup;
        private CanvasGroup canvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        protected virtual void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            IsOpen = true;

            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(Fade(1f));
        }

        public virtual void Hide()
        {
            IsOpen = false;

            if (gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
                StartCoroutine(Fade(0f, () => gameObject.SetActive(false)));
            }
            else
            {
                canvasGroup.alpha = 0f;
                SetInteraction(false);
                gameObject.SetActive(false);
            }
        }

        public void ShowInstant()
        {
            IsOpen = true;

            gameObject.SetActive(true);
            StopAllCoroutines();
            SetInteraction(true);
            canvasGroup.alpha = 1f;
        }

        public void HideInstant()
        {
            IsOpen = false;
        
            StopAllCoroutines();
            canvasGroup.alpha = 0f;
            SetInteraction(false);
            gameObject.SetActive(false);
        }

        private IEnumerator Fade(float targetAlpha, System.Action onComplete = null)
        {
            SetInteraction(false);

            float startAlpha = canvasGroup.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.SmoothStep(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;

            if (targetAlpha > 0f)
                SetInteraction(true);

            onComplete?.Invoke();
        }

        private void SetInteraction(bool setTo)
        {
            canvasGroup.interactable = setTo;
            canvasGroup.blocksRaycasts = setTo;
        }
    }
}