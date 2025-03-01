using Unity.Cinemachine;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    [SerializeField] private CinemachineCamera freeLookCamera;

    void Update()
    {
        transform.forward = freeLookCamera.transform.forward;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}