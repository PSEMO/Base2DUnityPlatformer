using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CheckPoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D _)
    {
        Events.InvokeCheckPointReached(transform.position);
    }
}