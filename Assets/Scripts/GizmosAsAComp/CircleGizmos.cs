using UnityEngine;

namespace PSEMO.GizmosAsAComp
{
    #if UNITY_EDITOR
    public class CircleGizmos : MonoBehaviour
    {
        [SerializeField] string label = "";
        [SerializeField] float circleRadius = 0.1f;

        private void OnDrawGizmos()
        {
            UnityEngine.Gizmos.DrawSphere(transform.position, circleRadius);
            DrawLabel(label);
        }

        private void DrawLabel(string text)
        {
            UnityEditor.Handles.Label(transform.position + Vector3.up * 0.2f, text);
        }
    }
    #endif
}