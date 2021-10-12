using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NotePanel : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 pos = Utils.PanelPosToFull(Utils.MousePos);
        NotePutManager.noteMaker.PutNote(pos);
    }
}

//-20.5, 11.5
//12.3, -6.9

//*0.8
//4.1, -2.3

//- 20.5, 11.5
//20.5, -11.5