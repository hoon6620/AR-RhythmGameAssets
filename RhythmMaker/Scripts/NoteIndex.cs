using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteIndex : MonoBehaviour
{
    public int id { get; private set; }
    public NoteSpawn noteSpawn { get; private set; }

    [SerializeField] TMP_Text idText;
    [SerializeField] TMP_InputField minutInput;
    [SerializeField] TMP_InputField secondInput;

    [SerializeField] TMP_InputField xPosInput;
    [SerializeField] TMP_InputField yPosInput;

    [SerializeField] TMP_Dropdown typeSelector;



    public void SetUp(int id, NoteSpawn noteSpawn)
    {
        this.id = id;
        this.noteSpawn = (noteSpawn);
        idText.text = id.ToString("D4");

        minutInput.text = ((int)noteSpawn.noteTime / 60).ToString();
        secondInput.text = (noteSpawn.noteTime % 60).ToString("N2");

        xPosInput.text = noteSpawn.notePos.x.ToString("N2");
        yPosInput.text = noteSpawn.notePos.y.ToString("N2");

        typeSelector.value = (int)noteSpawn.noteType;
    }

    public void ChangeTime()
    {
        int minut, second;

        if (int.TryParse(minutInput.text, out minut) && int.TryParse(secondInput.text, out second))
        {
            noteSpawn.noteTime = 60 * minut + second;
        }
        else
        {
            minutInput.text = ((int)noteSpawn.noteTime / 60).ToString();
            secondInput.text = ((int)noteSpawn.noteTime % 60).ToString();
        }
        NotePutManager.noteMaker.ChangeNoteIndex();
    }

    public void ChangePos()
    {
        float x, y;

        if (float.TryParse(xPosInput.text, out x) && float.TryParse(yPosInput.text, out y))
        {
            noteSpawn.notePos = new Vector2(x, y);
        }
        else
        {
            xPosInput.text = noteSpawn.notePos.x.ToString();
            yPosInput.text = noteSpawn.notePos.y.ToString();
        }
    }

    public void ChangeType()
    {
        noteSpawn.noteType = (NoteType)typeSelector.value;
    }

    public void Delete()
    {
        NotePutManager.noteMaker.DeleteNote(id);
    }
}
