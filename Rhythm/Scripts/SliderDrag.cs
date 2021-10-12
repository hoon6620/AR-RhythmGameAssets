using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        NotePutManager.noteMaker.DragBegin();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        NotePutManager.noteMaker.DragEnd();
    }

}
