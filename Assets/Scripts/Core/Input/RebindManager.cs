using UnityEngine;
using UnityEngine.InputSystem;

namespace PSEMO.Input
{
    public static class RebindManager
    {
        private const string REBINDS_KEY = "input_rebinds";

        public static void SaveOverrides(InputActionAsset asset)
        {
            var rebinds = asset.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(REBINDS_KEY, rebinds);
            PlayerPrefs.Save();
        }

        public static void LoadOverrides(InputActionAsset asset)
        {
            if (PlayerPrefs.HasKey(REBINDS_KEY))
            {
                var rebinds = PlayerPrefs.GetString(REBINDS_KEY);
                asset.LoadBindingOverridesFromJson(rebinds);
            }
        }

        public static void ResetOverrides(InputActionAsset asset)
        {
            asset.RemoveAllBindingOverrides();
            PlayerPrefs.DeleteKey(REBINDS_KEY);
            PlayerPrefs.Save();
        }
    }
}
