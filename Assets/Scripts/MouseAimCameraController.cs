using System.Collections;
using UnityEngine;
using Cinemachine;

public class MouseAimCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float maxOffset = 2f; // Maximum offset from the player
    public float sensitivity = 1f; // Mouse sensitivity

    public float resetDuration = 0.5f; // Duration of the reset animation

    private Vector3 originalOffset;
    private Vector3 targetOffset;
    private bool aiming;

    void Start()
    {
        // Store the original offset of the virtual camera
        originalOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }
    void Update()
    {
        // Check if the Shift key is pressed
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            aiming = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            aiming = false;
            //virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = originalOffset;
            StartCoroutine(ResetCameraOffset());
        }

        // If aiming, move the camera based on mouse movement
        if (aiming)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // Calculate the new offset based on mouse movement
            Vector3 currentOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            Vector3 newOffset = new Vector3(
                Mathf.Clamp(currentOffset.x + mouseX, -maxOffset, maxOffset),
                Mathf.Clamp(currentOffset.y + mouseY, -maxOffset, maxOffset),
                currentOffset.z
            );

            // Apply the new offset to the virtual camera
            virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = newOffset;
        }
    }
    IEnumerator ResetCameraOffset()
    {
        float elapsedTime = 0f;
        Vector3 startOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        while (elapsedTime < resetDuration)
        {
            // Interpolate between the current offset and the original offset
            virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(startOffset, originalOffset, elapsedTime / resetDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera offset is exactly the original offset when the interpolation is complete
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = originalOffset;
    }
}
