using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KillBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D _)
    {
        Events.InvokePlayerDeath();
    }
}