using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty { EASY, NORMAL, HALD, EXPERT }
public enum Judge { Perfact, Good, Bad, Miss }

public class NoteManager : MonoBehaviour
{
    public static NoteManager Inst;
    bool onPlay;
    private void Awake() => Inst = this;

    [SerializeField]
    GameObject NotePrefab;

    [SerializeField]
    MusicSet nowMusic;
    AudioSource ac;
    Difficulty nowDifficulty;

    private void Start()
    {
        onPlay = false;
        ac = gameObject?.GetComponent<AudioSource>();
        if (ac == null)
            ac = gameObject.AddComponent<AudioSource>();

        StartCoroutine(SongCo());
    }

    void SetSong(MusicSet music, Difficulty difficulty)
    {
        nowMusic = music;
        nowDifficulty = difficulty;
        StartCoroutine(SongCo());
    }

    IEnumerator SongCo()
    {
        var notes = nowMusic.noteSpawns;
        float timer = 0.0f;
        ac.PlayOneShot(nowMusic.music);
        foreach (var note in notes)
        {
            while (note.noteTime - Note.justTime >= timer)
            {
                yield return new WaitForFixedUpdate();
                timer += Time.fixedDeltaTime;
            }
            Instantiate(NotePrefab, note.notePos, Quaternion.identity).GetComponent<Note>().Setup(note.noteType);
        }
    }
}
