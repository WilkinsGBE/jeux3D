using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    // Variables

    public Transform player;

    public float mouseSensitivity = 2f;

    float cameraVerticalRotation = 0f;

    bool lockedCursor = true;

    void Start()

    {
        // Verrouiller et masquer le curseur

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()

    {

        // Collecte des entrées souris

        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;

        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotation de la caméra autour de son axe X local

        cameraVerticalRotation -= inputY;

        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        // Rotation du joueur et de la caméra autour de son axe Y

        player.Rotate(Vector3.up * inputX);

    }
}
