using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PSEMO.Events;

namespace PSEMO.UI
{
    public class NavigationPanel : MonoBehaviour
    {
        [SerializeField] private List<BasePanel> subPanels;

        [SerializeField] private TextMeshProUGUI leftText;
        [SerializeField] private TextMeshProUGUI middleText;
        [SerializeField] private TextMeshProUGUI rightText;

        [SerializeField] private int currentIndex = 0;

        private void OnEnable()
        {
            UIEvents.OnInputRight += NextPanel;
            UIEvents.OnInputLeft += PreviousPanel;
            
            for (int i = 0; i < subPanels.Count; i++)
            {
                if (i == currentIndex)
                {
                    subPanels[i].Init();
                    subPanels[i].ShowInstant();
                }
                else
                {
                    subPanels[i].Init();
                    subPanels[i].HideInstant();
                }
            }

            UpdateUI();
        }

        private void OnDisable()
        {
            UIEvents.OnInputRight -= NextPanel;
            UIEvents.OnInputLeft -= PreviousPanel;
        }

        private void NextPanel()
        {
            subPanels[currentIndex].Hide(SlideDirection.Left);

            currentIndex = (currentIndex + 1) % subPanels.Count;

            subPanels[currentIndex].Show(SlideDirection.Right);
            
            UpdateUI();
        }

        private void PreviousPanel()
        {
            subPanels[currentIndex].Hide(SlideDirection.Right);

            currentIndex = (currentIndex - 1 + subPanels.Count) % subPanels.Count;

            subPanels[currentIndex].Show(SlideDirection.Left);

            UpdateUI();
        }

        private void UpdateUI()
        {
            int leftIndex = (currentIndex - 1 + subPanels.Count) % subPanels.Count;
            int rightIndex = (currentIndex + 1) % subPanels.Count;

            leftText.text = subPanels[leftIndex].DisplayName;
            middleText.text = subPanels[currentIndex].DisplayName;
            rightText.text = subPanels[rightIndex].DisplayName;
        }
    }
}