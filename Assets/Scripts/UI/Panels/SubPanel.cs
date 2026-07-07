using UnityEngine;

namespace PSEMO.UI
{
    public class SubPanel : BasePanel
    {
        [SerializeField] string subPanelDisplayName;

        public string DisplayName => subPanelDisplayName;
    }
}