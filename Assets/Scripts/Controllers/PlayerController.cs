using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerSO data;

    void Start()
    {
        CameraManager.instance.AddTarget("player", transform, data.camDivisor);
    }

    void OnDestroy()
    {
        CameraManager.instance.RemoveTarget("player");
    }
}