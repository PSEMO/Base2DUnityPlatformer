using UnityEditor;
using PSEMO.UI;

namespace PSEMO.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(NavigationPanel))]
    public class NavigationPanelEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SerializedProperty textBoxesProp = serializedObject.FindProperty("textBoxes");

            if (textBoxesProp != null)
            {
                if (textBoxesProp.arraySize % 2 == 0)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("Requirement: There must be an odd number of Text Boxes for the Navigation Panel to function correctly.", MessageType.Error);
                }
            }
        }
    }
#endif
}