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
        
        private Vector2[] positions;

        void Awake()
        {
            if (textBoxes.Count % 2 == 0)
            {
                Debug.LogError("There has to be an odd number of text boxes!");
                Destroy(this);
                return;
            }

            positions = new Vector2[textBoxes.Count];
            for (int i = 0; i < textBoxes.Count; i++)
            {
                positions[i] = textBoxes[i].rectTransform.anchoredPosition;
                textBoxes[i].transitionPlayer.Init();
                textBoxes[i].transitionPlayer.UpdateShowPos();
                textBoxes[i].transitionPlayer.ApplyInstant(true);
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
            for (int i = 0; i < textBoxes.Count; i++)
            {
                var box = textBoxes[i];
                UpdateTextBoxText(box, i);
                box.transitionPlayer.UpdateShowPos(positions[i]);
                box.transitionPlayer.ApplyInstant(true);
            }

            SlideDirection hideDir = isNext ? SlideDirection.Left : SlideDirection.Right;
            SlideDirection showDir = isNext ? SlideDirection.Right : SlideDirection.Left;

            subPanels[currentIndex].Hide(hideDir);
            currentIndex = (currentIndex + (isNext ? 1 : -1) + subPanels.Count) % subPanels.Count;
            subPanels[currentIndex].Show(showDir);

            //wrap around the last object within the list
            int targetIdx = isNext ? textBoxes.Count - 1 : 0;
            int removeIdx = isNext ? 0 : textBoxes.Count - 1;
            var wrappedBox = textBoxes[removeIdx];
            textBoxes.RemoveAt(removeIdx);
            textBoxes.Insert(targetIdx, wrappedBox);

            for (int i = 0; i < textBoxes.Count; i++)
            {
                var box = textBoxes[i];
                var player = box.transitionPlayer;

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
                        player.Play(true, null, showDir, 2); 
                    }, hideDir, 2);
                }
                else
                {
                    player.PlayToPosAndShow(positions[i], () => player.UpdateShowPos());
                }
            }
        }

        private void UpdateTextBoxText(TransitionTextCouple box, int index)
        {
            int panelIndex = (currentIndex + index - textBoxes.Count / 2) % subPanels.Count;
            if (panelIndex < 0) panelIndex += subPanels.Count;
            box.tmp.text = subPanels[panelIndex].DisplayName;
        }

        private void UpdateUI()
        {
            for (int i = 0; i < textBoxes.Count; i++)
            {
                UpdateTextBoxText(textBoxes[i], i);
            }
        }
    }
}