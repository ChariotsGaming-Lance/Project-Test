using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BombCrossHair : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("Required")]
    [SerializeField]
    private Transform crossHair = null;

    Vector3 crossHairWorldPosition = Vector3.zero;
    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100);

        if (null != hit.collider)
            crossHair.position = hit.point;        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
}
