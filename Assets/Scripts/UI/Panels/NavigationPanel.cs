using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PSEMO.Events;

namespace PSEMO.UI
{
    public class NavigationPanel : MonoBehaviour
    {
        [SerializeField] private List<BasePanel> subPanels;
        [SerializeField] private List<ElementTransitionPlayer> textPlayers;
        [SerializeField] private int currentIndex = 0;
        
        private Vector2[] positions;
        private TextMeshProUGUI[] textComponents;

        void Awake()
        {
            if (textPlayers.Count % 2 == 0)
            {
                Debug.LogError("There has to be an odd number of text boxes!");
                Destroy(this);
                return;
            }

            positions = new Vector2[textPlayers.Count];
            textComponents = new TextMeshProUGUI[textPlayers.Count];

            for (int i = 0; i < textPlayers.Count; i++)
            {
                var player = textPlayers[i];
                player.Init();
                
                positions[i] = ((RectTransform)player.transform).anchoredPosition;
                textComponents[i] = player.GetComponent<TextMeshProUGUI>();
                
                player.PlayInstantTo(positions[i]);
            }
            
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

        private void OnEnable()
        {
            UIEvents.OnInputRight += NextPanel;
            UIEvents.OnInputLeft += PreviousPanel;
        }

        private void OnDisable()
        {
            UIEvents.OnInputRight -= NextPanel;
            UIEvents.OnInputLeft -= PreviousPanel;
        }

        private void NextPanel() => Navigate(true);
        private void PreviousPanel() => Navigate(false);

        private void Navigate(bool isNext)
        {
            SlideDirection hideDir = isNext ? SlideDirection.Left : SlideDirection.Right;
            SlideDirection showDir = isNext ? SlideDirection.Right : SlideDirection.Left;

            subPanels[currentIndex].Hide(hideDir);
            currentIndex = (currentIndex + (isNext ? 1 : -1) + subPanels.Count) % subPanels.Count;
            subPanels[currentIndex].Show(showDir);

            // wrap around the last object within the list
            int targetIdx = isNext ? textPlayers.Count - 1 : 0;
            int removeIdx = isNext ? 0 : textPlayers.Count - 1;
            
            var wrappedPlayer = textPlayers[removeIdx];
            textPlayers.RemoveAt(removeIdx);
            textPlayers.Insert(targetIdx, wrappedPlayer);
            
            var wrappedText = textComponents[removeIdx];
            var newTextList = new List<TextMeshProUGUI>(textComponents);
            newTextList.RemoveAt(removeIdx);
            newTextList.Insert(targetIdx, wrappedText);
            textComponents = newTextList.ToArray();

            for (int i = 0; i < textPlayers.Count; i++)
            {
                var player = textPlayers[i];

                if (i == targetIdx)
                {
                    int tempI = i;

                    Vector2 hideTargetPos = player.GetHiddenPos(positions[tempI], hideDir);

                    player.PlayHideTo(hideTargetPos, () => 
                    {
                        UpdateTextBoxText(tempI);
                        Vector2 showStartPos = player.GetHiddenPos(positions[tempI], showDir);
                        
                        player.PlayInstantHiddenAt(showStartPos);
                        player.PlayShowFrom(showStartPos, positions[tempI], null, 2f);
                    }, 2f);
                }
                else
                {
                    player.PlayTo(positions[i]);
                }
            }
        }

        private void UpdateTextBoxText(int index)
        {
            int panelIndex = (currentIndex + index - textPlayers.Count / 2) % subPanels.Count;
            if (panelIndex < 0) panelIndex += subPanels.Count;
            textComponents[index].text = subPanels[panelIndex].DisplayName;
        }

        private void UpdateUI()
        {
            for (int i = 0; i < textPlayers.Count; i++)
            {
                UpdateTextBoxText(i);
            }
        }
    }
}