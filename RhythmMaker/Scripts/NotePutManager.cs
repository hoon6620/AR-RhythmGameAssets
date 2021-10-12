using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotePutManager: MonoBehaviour
{
    public static NotePutManager noteMaker;

    [SerializeField]
    AudioClip music;
    AudioSource source;

    [SerializeField]
    MusicSet musicSet;

    float songLength;
    int noteCount;
    bool onPause;
    bool onDrag;
    NoteType selectedNoteType;

    [SerializeField]
    Text timeText;
    [SerializeField]
    Text typeText;
    [SerializeField]
    Text noteText;
    [SerializeField]
    Slider timeSlider;
    [SerializeField]
    ScrollRect notesScroll;
    [SerializeField]
    GameObject notePutPrefabs;
    [SerializeField]
    GameObject noteIndexPrefab;

    private void Awake()
    {
        if (noteMaker != null)
        {
            Destroy(gameObject);
            return;
        }
        noteMaker = this;

        noteCount = 0;
        onPause = true;
        onDrag = false;
        selectedNoteType = NoteType.HAND_RIGHT;

        musicSet.music = music;
        source = gameObject?.GetComponent<AudioSource>();
        if (source == null)
            source = gameObject.AddComponent<AudioSource>();
        source.clip = music;

        timeSlider.value = 0;
        SetTimeText();
        SetTypeText();
        SetNoteText();
    }

    private void Start()
    {
        Transform content = notesScroll.content.transform;
        for (int i = 0; i < musicSet.noteSpawns.Count; i++)
        {
            GameObject block = Instantiate(noteIndexPrefab);
            block.transform.SetParent(content);
            block.transform.localScale = Vector3.one;
            block.transform.localPosition = new Vector3(0, - i * 60, 0);

            block.GetComponent<NoteIndex>().SetUp(i, musicSet.noteSpawns[i]);
        }
    }

    private void Update()
    {
        if (!onDrag && !onPause)
        {
            timeSlider.value = (float)source.timeSamples / (float)music.samples;
            SetTimeText();
        }
        if (source.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (source.time + 5 < music.length)
                    source.time += 5;
                else
                {
                    source.time = 0;
                    source.Stop();
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (source.time > 5)
                    source.time -= 5;
                else
                    source.time = 0;
            }
            timeSlider.value = (float)source.timeSamples / (float)music.samples;
            SetTimeText();

            while (noteCount < musicSet.noteSpawns.Count && source.time > (musicSet.noteSpawns[noteCount].noteTime - OptionManager.NoteJustTime))
            {
                Debug.Log(noteCount);
                Instantiate(notePutPrefabs, Utils.FullPosToPanel(musicSet.noteSpawns[noteCount].notePos), Quaternion.identity).GetComponent<NotePut>().Setup(musicSet.noteSpawns[noteCount].noteType);
                noteCount++;
                SetNoteText();
            }
        }
        TypeChangeInput();
    }

    void TypeChangeInput()
    {
        bool changed = false;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedNoteType = NoteType.HAND_LEFT;
            changed = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedNoteType = NoteType.HAND_RIGHT;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedNoteType = NoteType.ELBOW_LEFT;
            changed = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            selectedNoteType = NoteType.ELBOW_RIGHT;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedNoteType = NoteType.KNEE_LEFT;
            changed = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            selectedNoteType = NoteType.KNEE_RIGHT;
            changed = true;
        }
        if (changed)
            SetTypeText();
    }

    public void Pause()
    {
        if (!source.isPlaying)
        {
            noteCount = 0;
            source.time = 0;
            onPause = false;
            source.Play();
            return;
        }

        onPause = !onPause;
        if (onPause)
        {
            source.Pause();
        }
        else
        {
            source.UnPause();
        }
    }

    public void DragBegin()
    {
        onDrag = true;
        source.Pause();
    }

    public void DragEnd()
    {
        Debug.Log(noteCount);
        if (source.time < (int)(timeSlider.value * music.length))
        {
            Debug.Log("+");
            Debug.Log(musicSet.noteSpawns.Count);
            while (noteCount < musicSet.noteSpawns.Count - 1 && (musicSet.noteSpawns[noteCount+1].noteTime - OptionManager.NoteJustTime) < (int)(timeSlider.value * music.length))
            {
                noteCount++;
            }
            if (noteCount == musicSet.noteSpawns.Count - 1 && (musicSet.noteSpawns[noteCount].noteTime - OptionManager.NoteJustTime) < (int)(timeSlider.value * music.length))
                noteCount++;
        }
        else
        {
            Debug.Log("-");
            while (noteCount > 0 && (musicSet.noteSpawns[noteCount-1].noteTime) > (int)(timeSlider.value * music.length))
            {
                noteCount--;
            }
        }
        SetNoteText();
        onDrag = false;
        source.time = timeSlider.value * music.length - 0.01f;
        SetTimeText();
        if (!onPause)
            source.UnPause();
    }

    void SetTimeText()
    {
        timeText.text = "Time: " + ((int)source.time / 60).ToString() + ":" + (source.time - (int)source.time / 60).ToString("N2") + " / " +
        ((int)music.length / 60).ToString() + ":" + (music.length - (int)music.length / 60).ToString("N2");
    }

    void SetTypeText()
    {
        typeText.text = "Selected type: " + selectedNoteType.ToString();
    }
    
    void SetNoteText()
    {
        noteText.text = "Notes: " + noteCount.ToString() + "/" + musicSet.noteSpawns.Count.ToString();
    }

    void SetScroll()
    {
        //notesScroll.content.
    }

    public void PutNote(Vector3 pos)
    {
        musicSet.noteSpawns.Add(new NoteSpawn(source.time, selectedNoteType, pos));

        GameObject block = Instantiate(noteIndexPrefab);
        block.transform.SetParent(notesScroll.content);
        block.transform.localScale = Vector3.one;
        block.transform.localPosition = new Vector3(0, -(notesScroll.content.childCount-1) * 60, 0);

        ChangeNoteIndex();
        SetNoteText();
    }

    public void DeleteNote(int id)
    {
        if (noteCount > id)
            noteCount--;

        musicSet.noteSpawns.RemoveAt(id);
        Destroy(notesScroll.content.GetChild(notesScroll.content.childCount-1).gameObject);
        ChangeNoteIndex();
        SetNoteText();
    }
    
    public void ChangeNoteIndex()
    {
        for (int i = 0; i < musicSet.noteSpawns.Count; i++)
        {
            notesScroll.content.GetChild(i).GetComponent<NoteIndex>().SetUp(i, musicSet.noteSpawns[i]);
        }
        NoteSpawnSort();
    }

    public void NoteSpawnSort()
    {
        musicSet.noteSpawns.Sort((x, y) =>
        {
            if (x.noteTime > y.noteTime)
                return 1;
            else
                return -1;
        });
    }
}
