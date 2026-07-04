using UnityEngine;

namespace PSEMO.GizmosAsAComp
{
    #if UNITY_EDITOR
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxGizmos : MonoBehaviour
    {
        [SerializeField] private Color boxColor = new(0, 0, 0, 0.3f);

        BoxCollider2D boxCollider;

        private void OnDrawGizmos()
        {
            if (boxCollider == null)
                boxCollider = GetComponent<BoxCollider2D>();
            
            if (boxCollider != null)
            {
                Matrix4x4 oldMatrix = Gizmos.matrix;
                Color oldColor = Gizmos.color;

                Gizmos.color = boxColor;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawCube(boxCollider.offset, boxCollider.size);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);

                Gizmos.matrix = oldMatrix;
                Gizmos.color = oldColor;
            }
        }
    }
    #endif
}