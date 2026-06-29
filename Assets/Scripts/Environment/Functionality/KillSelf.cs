using System.Collections;
using UnityEngine;

public class KillSelf : MonoBehaviour
{
    [SerializeField] float secondsToDieAfter = 2;

    void OnEnable()
    {
        StartCoroutine(KillSelfAfterSeconds(secondsToDieAfter));
    }

    IEnumerator KillSelfAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(secondsToDieAfter);

        Instantiator.Instance.DeSpawnObject(gameObject);
    }
}