using UnityEngine;

// Ce script permet de switch entre caméra 3e personne et FPS
public class CameraSwitcher : MonoBehaviour
{
    [Header("Third Person Camera")]
    public GameObject thirdPersonCamObject; // objet caméra 3rd person

    [Header("First Person Camera")]
    public Camera fpsCam; // caméra FPS

    // ===================== START =====================
    void Start()
    {
        Debug.Log(" CameraSwitcher START");
    }

    // ===================== UPDATE =====================
    void Update()
    {
        // Appuie sur V pour switch caméra
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(" Touche V détectée !");

            // Vérifie si caméras assignées
            if (thirdPersonCamObject == null || fpsCam == null)
            {
                Debug.LogError(" Caméras non assignées !");
                return;
            }

            // ===================== SWITCH =====================

            // Active / désactive caméra 3e personne
            thirdPersonCamObject.SetActive(!thirdPersonCamObject.activeSelf);

            // Active / désactive caméra FPS
            fpsCam.enabled = !fpsCam.enabled;

            Debug.Log("🔄 Switch caméra effectué !");
        }
    }
}