using UnityEngine;

public class Drag3DKey : MonoBehaviour
{
    public Transform keyholeTarget;
    public BossDoorTrigger bossDoor;

    private Camera cam;
    private bool dragging = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    dragging = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;

            float distance = Vector3.Distance(transform.position, keyholeTarget.position);

            if (distance < 0.5f)
            {
                bossDoor.TryOpenBossDoorFromUI();
            }
        }

        if (dragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2f;

            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
            transform.position = worldPos;
        }
    }
}