using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteType { HAND_RIGHT, HAND_LEFT, ELBOW_RIGHT, ELBOW_LEFT, KNEE_RIGHT, KNEE_LEFT };

[System.Serializable]
public class Note : MonoBehaviour
{
    public static float justTime = 1.5f;

    [SerializeField]
    Sprite handNoteSprite, elbowNoteSprite, kneeNoteSprite;
    
    NoteType noteType;

    [SerializeField]
    Color leftNoteColor, rightNoteColor;
    [SerializeField]
    Color leftJudgeColor, rightJudgeColor;
    
    [SerializeField]
    GameObject judgeEffectPrefab;

    float beginTime;
    bool hit;

    GameObject judgeObject;

    private void Start()
    {

    }

    // Start is called before the first frame update
    public void Setup(NoteType nType)
    {
        noteType = nType;
        hit = false;
        beginTime = Time.time;
        judgeObject = transform.GetChild(0).gameObject;

        switch(nType)
        {
            case NoteType.HAND_LEFT:
            case NoteType.HAND_RIGHT:
                gameObject.GetComponent<SpriteRenderer>().sprite = handNoteSprite;
                judgeObject.GetComponent<SpriteRenderer>().sprite = handNoteSprite;
                break;

            case NoteType.ELBOW_LEFT:
            case NoteType.ELBOW_RIGHT:
                gameObject.GetComponent<SpriteRenderer>().sprite = elbowNoteSprite;
                judgeObject.GetComponent<SpriteRenderer>().sprite = elbowNoteSprite;
                break;

            case NoteType.KNEE_LEFT:
            case NoteType.KNEE_RIGHT:
                gameObject.GetComponent<SpriteRenderer>().sprite = kneeNoteSprite;
                judgeObject.GetComponent<SpriteRenderer>().sprite = kneeNoteSprite;
                break;
        }

        switch (nType)
        {
            case NoteType.HAND_LEFT:
            case NoteType.ELBOW_LEFT:
            case NoteType.KNEE_LEFT:
                gameObject.GetComponent<SpriteRenderer>().color = leftNoteColor;
                judgeObject.GetComponent<SpriteRenderer>().color = leftJudgeColor;
                break;

            default:
                gameObject.GetComponent<SpriteRenderer>().color = rightNoteColor;
                judgeObject.GetComponent<SpriteRenderer>().color = rightJudgeColor;
                break;
        }

        StartCoroutine(EffectCo());
    }

    IEnumerator EffectCo()
    {
        Judge judge;
        while (Time.time < beginTime + justTime && !hit)
        {
            judgeObject.transform.localScale -= (Vector3)Vector2.one * (2 * Time.deltaTime/justTime);
            yield return new WaitForEndOfFrame();
        }

        while (Time.time < beginTime + 2 * justTime && !hit)
        {
            transform.localScale -= (Vector3)Vector2.one * (Time.deltaTime/justTime);
            yield return new WaitForEndOfFrame();
        }

        if (hit)
        {
            float hitScore = Time.time - beginTime - justTime;
            Debug.Log(hitScore);
            hitScore = hitScore > 0 ? hitScore : -hitScore;
            Debug.Log(hitScore);
            if (hitScore < justTime / 10)
            {
                judge = Judge.Perfact;
            }
            else if (hitScore < justTime / 3)
            {
                judge = Judge.Good;
            }
            else
                judge = Judge.Bad;
        }
        else
        {
            judge = Judge.Miss;
        }

        GameManager.NoteJudge(judge);
        Instantiate(judgeEffectPrefab,transform.position, Quaternion.identity).GetComponent<JudgeEffect>().Setup(judge);
        Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitRange")
        {
            if(collision.gameObject.GetComponent<HitRange>().noteType == this.noteType)
            {
                hit = true;
            }
        }
    }
}

[System.Serializable]
public class NoteSpawn
{
    public float noteTime;
    public NoteType noteType;
    public Vector2 notePos;

    public NoteSpawn(float noteTime, NoteType noteType, Vector2 notePos)
    {
        this.noteTime = noteTime;
        this.noteType = noteType;
        this.notePos = notePos;
    }

    public NoteSpawn(NoteSpawn other)
    {
        this.noteTime = other.noteTime;
        this.noteType = other.noteType;
        this.notePos = other.notePos;
    }
}