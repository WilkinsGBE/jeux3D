using UnityEngine;

public class ThirdPersonCameraLook : MonoBehaviour
{
    public float mouseSensitivity = 150f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private float yaw;
    private float pitch;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        Input.ResetInputAxes();
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.unscaledDeltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}