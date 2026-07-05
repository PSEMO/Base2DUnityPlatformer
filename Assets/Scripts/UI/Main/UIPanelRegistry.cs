using System.Collections.Generic;
using UnityEngine;

namespace PSEMO.UI
{
    public class UIPanelRegistry : MonoBehaviour
    {
        [SerializeField] private List<Panel> panels; 
        private Dictionary<PanelType, Panel> panelDict;

        private void Awake()
        {
            panelDict = new Dictionary<PanelType, Panel>();

            foreach (var panel in panels)
            {
                panel.HideInstant();
                panelDict.Add(panel.Type, panel);
            }
        }

        public Panel GetPanel(PanelType type)
        {
            if (panelDict.TryGetValue(type, out var panel))
                return panel;
            
            Debug.LogWarning($"[UIPanelRegistry] Panel of type {type} not found in registry!");
            return null;
        }
    }
}