using UnityEngine;
using Cinemachine;

public class CameraRotation : MonoBehaviour
{
    public float sensitivity = 2f; // Mouse sensitivity
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;

        RotateCamera(mouseX);
    }

    private void RotateCamera(float mouseX)
    {
        // Get the current rotation
        Vector3 currentRotation = virtualCamera.transform.rotation.eulerAngles;

        // Calculate the new rotation only around the Z-axis
        float newZRotation = currentRotation.z - mouseX;

        // Apply the new rotation to the camera
        virtualCamera.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, newZRotation);
    }
}
