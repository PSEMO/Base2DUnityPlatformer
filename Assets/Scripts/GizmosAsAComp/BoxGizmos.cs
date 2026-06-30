using UnityEngine;

namespace PSEMO.GizmosAsAComp
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxGizmos : MonoBehaviour
    {
        [SerializeField] private Color boxColor = new Color(0, 0, 0, 0.3f);

        private void OnDrawGizmos()
        {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                Gizmos.color = boxColor;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawCube(boxCollider.offset, boxCollider.size);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
            }
        }
    }
}