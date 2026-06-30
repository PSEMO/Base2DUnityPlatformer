using System.Collections;
using UnityEngine;
using PSEMO.Core.Management;

namespace PSEMO.Environment.Functionality
{
    public class KillSelf : MonoBehaviour
    {
        [SerializeField] float secondsToDieAfter = 2;

        void OnEnable()
        {
            StartCoroutine(KillSelfAfterSeconds(secondsToDieAfter));
        }

        IEnumerator KillSelfAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            Instantiator.Instance.DeSpawnObject(gameObject);
        }
    }
}