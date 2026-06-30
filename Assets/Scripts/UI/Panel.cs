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

        private CanvasGroup canvasGroup;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public virtual void Show()
        {
            IsOpen = true;

            gameObject.SetActive(true);
            if (gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
                StartCoroutine(Fade(1f));
            }
            else
            {
                if (canvasGroup != null) canvasGroup.alpha = 1f;
            }
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
                if (canvasGroup != null) canvasGroup.alpha = 0f;
                gameObject.SetActive(false);
            }
        }

        public void ShowInstant()
        {
            IsOpen = true;

            gameObject.SetActive(true);
            if (canvasGroup != null) canvasGroup.alpha = 1f;
        }

        public void HideInstant()
        {
            IsOpen = false;
        
            if (canvasGroup != null) canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        private IEnumerator Fade(float targetAlpha, System.Action onComplete = null)
        {
            float startAlpha = canvasGroup.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.SmoothStep(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            onComplete?.Invoke();
        }
    }
}
