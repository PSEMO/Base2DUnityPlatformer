using UnityEngine;
using TMPro;

namespace PSEMO.UI
{
    public class UnpauseCountdownUI : MonoBehaviour
    {
        [SerializeField] UISO Data;
        [SerializeField] private TextMeshProUGUI countdownText;
        private float timer;

        private void OnEnable()
        {
            timer = Data.returningFromPauseCooldown;
            UpdateText();
        }

        private void Update()
        {
            if (timer > 0f)
            {
                timer -= Time.unscaledDeltaTime;
                if (timer < 0f) timer = 0f;
            
                UpdateText();
            }
        }

        private void UpdateText()
        {
            if (countdownText != null)
            {
                countdownText.text = timer.ToString("F1");
            }
        }
    }
}
