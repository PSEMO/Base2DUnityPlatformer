using System.Collections;
using UnityEngine;

public class EnableOnInteract : MonoBehaviour, IInteractable
{    
    [Header("Object to enable")]
    [SerializeField] private GameObject objectToEnable;

    [Header("How long should the object be enabled for? (0 or less is infinite)")]
    [SerializeField] private float duration;

    private Coroutine EnablingRoutine = null;

    public void OnInteracted()
    {
        if (duration <= 0)
        {
            objectToEnable.SetActive(true);
        }
        else
        {
            if (EnablingRoutine != null)
                StopCoroutine(EnablingRoutine);
            
            EnablingRoutine = StartCoroutine(EnableObject(duration));
        }
    }

    IEnumerator EnableObject(float duration)
    {
        objectToEnable.SetActive(true);

        yield return new WaitForSeconds(duration);

        objectToEnable.SetActive(false);
    }
}