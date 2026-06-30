using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnableWhileContact : MonoBehaviour
{
    [Header("Object to enable")]
    [SerializeField] private GameObject objectToEnable;

    void OnTriggerEnter2D(Collider2D _)
    {
        objectToEnable.SetActive(true);
    }
    void OnCollisionEnter2D(Collision2D _)
    {
        objectToEnable.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D _)
    {
        objectToEnable.SetActive(false);
    }
    void OnCollisionExit2D(Collision2D _)
    {
        objectToEnable.SetActive(false);
    }
}