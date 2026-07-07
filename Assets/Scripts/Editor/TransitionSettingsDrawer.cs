using UnityEditor;
using PSEMO.UI;

namespace PSEMO.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(TransitionSO))]
    public class TransitionSettingsSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var transitionTypeProp = serializedObject.FindProperty("transitionType");
            var durationProp = serializedObject.FindProperty("duration");
            var useSmoothingProp = serializedObject.FindProperty("useSmoothing");
            var slideDirectionProp = serializedObject.FindProperty("slideDirection");
            var slideDistanceProp = serializedObject.FindProperty("slideDistance");
            var hiddenScaleProp = serializedObject.FindProperty("hiddenScale");
            var hiddenAlphaProp = serializedObject.FindProperty("hiddenAlpha");

            EditorGUILayout.PropertyField(transitionTypeProp);
            EditorGUILayout.Space(4);

            EditorGUILayout.PropertyField(durationProp);
            EditorGUILayout.PropertyField(useSmoothingProp);

            TransitionType flags = (TransitionType)transitionTypeProp.intValue;

            if (flags.HasFlag(TransitionType.Slide))
            {
                EditorGUILayout.Space(8);
                EditorGUILayout.LabelField("Slide", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(slideDirectionProp);
                EditorGUILayout.PropertyField(slideDistanceProp);
                EditorGUI.indentLevel--;
            }

            if (flags.HasFlag(TransitionType.Scale))
            {
                EditorGUILayout.Space(8);
                EditorGUILayout.LabelField("Scale", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(hiddenScaleProp);
                EditorGUI.indentLevel--;
            }

            if (flags.HasFlag(TransitionType.Fade))
            {
                EditorGUILayout.Space(8);
                EditorGUILayout.LabelField("Fade", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(hiddenAlphaProp);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}