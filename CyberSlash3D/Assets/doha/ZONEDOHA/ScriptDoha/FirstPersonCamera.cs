using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 200f; // sensibilité de la souris

    private Transform eyes;   // position des yeux du joueur
    private Transform player;  // référence au joueur (root)

    private float xRotation = 0f; // rotation verticale (haut / bas)

    void Start()
    {
        // 🔒Bloque et cache le curseur au centre de l’écran
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //  Cherche l’objet "Eyes" dans la scène
        GameObject e = GameObject.FindWithTag("Eyes");

        if (e == null)
        {
            Debug.LogError("❌ Aucun objet avec le tag 'Eyes' !");
            return;
        }

        //  On récupère les références
        eyes = e.transform;
        player = eyes.root;

        //  On attache la caméra aux yeux du joueur
        transform.SetParent(eyes);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Debug.Log("👀 FPS Camera prête !");
    }

    void Update()
    {
        //  sécurité si pas trouvé
        if (eyes == null || player == null) return;

        // 🖱️ Lecture de la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotation verticale (regarder haut/bas)
        xRotation -= mouseY;

        //  limite pour éviter de tourner à 360°
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        //  applique rotation caméra (haut/bas)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //  rotation horizontale du joueur (gauche/droite)
        player.Rotate(Vector3.up * mouseX);
    }
}