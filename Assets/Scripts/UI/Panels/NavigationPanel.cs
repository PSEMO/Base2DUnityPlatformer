using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PSEMO.Events;

namespace PSEMO.UI
{
    public class NavigationPanel : MonoBehaviour
    {
        [SerializeField] private List<BasePanel> subPanels;
        [SerializeField] private List<ElementTransitionPlayer> Players;
        [SerializeField] private int currentIndex = 0;
        
        private Vector2[] positions;

        void Awake()
        {
            if (Players.Count % 2 == 0)
            {
                Debug.LogError("There has to be an odd number of text boxes!");
                Destroy(this);
                return;
            }

            positions = new Vector2[Players.Count];
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].Init();
                positions[i] = Players[i].rectTransform.anchoredPosition;
                Players[i].UpdateShowPos();
                if (i == Players.Count / 2)
                {
                    Players[i].ApplyInstant(true);
                }
                else
                {
                    Players[i].ApplyHalfInstant();
                }
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

        private void NextPanel() => Navigate(true);
        private void PreviousPanel() => Navigate(false);

        private void Navigate(bool isNext)
        {
            //reset half-done animations before start.
            for (int i = 0; i < Players.Count; i++)
            {
                var box = Players[i];
                UpdateTextBoxText(box, i);
                box.UpdateShowPos(positions[i]);
                if (i == Players.Count / 2)
                {
                    box.ApplyInstant(true);
                }
                else
                {
                    box.ApplyHalfInstant();
                }
            }

            SlideDirection hideDir = isNext ? SlideDirection.Left : SlideDirection.Right;
            SlideDirection showDir = isNext ? SlideDirection.Right : SlideDirection.Left;

            subPanels[currentIndex].Hide(hideDir);
            currentIndex = (currentIndex + (isNext ? 1 : -1) + subPanels.Count) % subPanels.Count;
            subPanels[currentIndex].Show(showDir);

            //wrap around the last object within the list
            int targetIdx = isNext ? Players.Count - 1 : 0;
            int removeIdx = isNext ? 0 : Players.Count - 1;
            var wrappedBox = Players[removeIdx];
            Players.RemoveAt(removeIdx);
            Players.Insert(targetIdx, wrappedBox);

            for (int i = 0; i < Players.Count; i++)
            {
                var box = Players[i];
                var player = box;

                if (i == targetIdx)
                {
                    int tempI = i;

                    //play anim till it dissappears to side then reset its position to other side and
                    //play an anim again to show it and move it towards the new right position
                    player.Play(false, () => 
                    {
                        UpdateTextBoxText(box, tempI);
                        player.UpdateShowPos(positions[tempI]); 
                        player.ApplyInstant(false, showDir);
                        player.PlayCustom(positions[tempI], player.halfScale, player.halfAlpha, null, 2); 
                    }, hideDir, 2);
                }
                else
                {
                    bool isCenter = i == Players.Count / 2;
                    player.PlayToPosAndShow(positions[i], isCenter, () => player.UpdateShowPos());
                }
            }
        }

        private void UpdateTextBoxText(ElementTransitionPlayer box, int index)
        {
            int panelIndex = (currentIndex + index - Players.Count / 2) % subPanels.Count;
            if (panelIndex < 0) panelIndex += subPanels.Count;
            box.tmp.text = subPanels[panelIndex].DisplayName;
        }

        private void UpdateUI()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                UpdateTextBoxText(Players[i], i);
            }
        }
    }
}