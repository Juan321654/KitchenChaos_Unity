using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }

    [SerializeField] private Mode mode = Mode.CameraForward;
    private void LateUpdate()
    {
        if (mode == Mode.LookAt) transform.LookAt(Camera.main.transform);
        if (mode == Mode.LookAtInverted) transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        if (mode == Mode.CameraForward) transform.forward = Camera.main.transform.forward;
        if (mode == Mode.CameraForwardInverted) transform.forward = -Camera.main.transform.forward;
    }
}
