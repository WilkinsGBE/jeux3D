using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIKey : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform keyholeTarget;
    public BossDoorTrigger bossDoor;
    public float unlockDistance = 80f;

    private RectTransform keyRect;
    private Canvas canvas;
    private Vector2 startPosition;

    void Awake()
    {
        keyRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPosition = keyRect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = keyRect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        keyRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float distance = Vector2.Distance(keyRect.position, keyholeTarget.position);

        if (distance <= unlockDistance)
        {
            bossDoor.TryOpenBossDoorFromUI();
        }
        else
        {
            keyRect.anchoredPosition = startPosition;
        }
    }
}