using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnableOnContract : MonoBehaviour
{
    [Header("Object to enable")]
    [SerializeField] private GameObject objectToEnable;

    [Header("How long should the object be enabled for?")]
    [SerializeField] private float duration;

    private Coroutine EnablingRoutine = null;

    void OnTriggerEnter2D(Collider2D _)
    {
        if (EnablingRoutine != null)
            StopCoroutine(EnablingRoutine);
        
        EnablingRoutine = StartCoroutine(EnableObject(duration));
    }

    IEnumerator EnableObject(float duration)
    {
        objectToEnable.SetActive(true);

        yield return new WaitForSeconds(duration);

        objectToEnable.SetActive(false);
    }
}