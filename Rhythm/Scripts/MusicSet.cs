using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MusicSet", menuName = "Rythm/MusicSet")]
public class MusicSet : ScriptableObject
{
    public string songID;
    public string songName;
    public AudioClip music;

    public int level;
    public List<NoteSpawn> noteSpawns;
}