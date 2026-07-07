using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PSEMO.Events;

namespace PSEMO.UI
{
    public class NavigationPanel : MonoBehaviour
    {
        [SerializeField] private List<BasePanel> subPanels;
        [SerializeField] private List<TransitionTextCouple> textBoxes;
        [SerializeField] private int currentIndex = 0;

        void Awake()
        {
            if (textBoxes.Count % 2 == 0)
            {
                Debug.LogError("There has to be an odd number of text boxes!");
                Destroy(this);
            }
        }

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
            int middleIndex = textBoxes.Count / 2;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                int offset = i - middleIndex;
                int panelIndex = (currentIndex + offset) % subPanels.Count;
                
                if (panelIndex < 0)
                {
                    panelIndex += subPanels.Count;
                }

                textBoxes[i].text.text = subPanels[panelIndex].DisplayName;
            }
        }
    }
}