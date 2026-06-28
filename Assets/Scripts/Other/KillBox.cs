using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KillBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D _)
    {
        Events.InvokePlayerDeath();
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(boxCollider.offset, boxCollider.size);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
        }
    }
}