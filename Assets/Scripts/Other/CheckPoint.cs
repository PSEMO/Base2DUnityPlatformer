using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    public Vector3 SpawnPos => spawnPoint.position;

    private void OnTriggerEnter2D(Collider2D _)
    {
        Events.InvokeCheckPointReached(SpawnPos);
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(boxCollider.offset, boxCollider.size);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
        }
    }
}