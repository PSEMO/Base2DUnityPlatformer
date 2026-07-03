using UnityEditor;
using UnityEngine;
using PSEMO.Core.Persistence;

namespace PSEMO.Editor
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(Persists), true)]
    [CanEditMultipleObjects]
    public class PersistsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This object should not be destroyed or instantiated mid-gameplay.", MessageType.Warning);

            EditorGUILayout.Space();

            base.OnInspectorGUI();
            
            if (GUILayout.Button("Generate New ID", GUILayout.Height(30)))
            {
                foreach (var t in targets)
                {
                    Persists script = (Persists)t;
                    Undo.RecordObject(script, "Generate New ID");
                    script.GenerateId();
                    EditorUtility.SetDirty(script);
                }
            }
        }
    }
    #endif
}