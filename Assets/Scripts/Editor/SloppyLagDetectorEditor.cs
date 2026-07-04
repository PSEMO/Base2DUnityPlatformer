using UnityEditor;
using PSEMO.Diagnostic;

namespace PSEMO.Editor
{
    [CustomEditor(typeof(SloppyLagDetector))]
    public class SloppyLagDetectorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox(
                "OnSceneLoaded measures from:\n" +
                "Very begining of the last scenes death (before OnDestroy and stuff)\n" +
                "TO\n" +
                "Just before the next scenes Start (After Awake and OnEnabled)\n" +
                "--------\n" +
                "Update measures:\n" +
                "Everything of a frame. (including; Awake/OnEnabled/Start for the first frame)", 
                UnityEditor.MessageType.Info);

            DrawDefaultInspector();
        }
    }
}