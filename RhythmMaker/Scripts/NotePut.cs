using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePut : MonoBehaviour
{
    [SerializeField]
    Sprite handNoteSprite, elbowNoteSprite, kneeNoteSprite;

    NoteType noteType;

    [SerializeField]
    Color leftNoteColor, rightNoteColor;
    [SerializeField]
    Color leftJudgeColor, rightJudgeColor;

    [SerializeField]
    GameObject judgeEffectPrefab;

    float justTime;

    GameObject judgeObject;

    // Start is called before the first frame update
    public void Setup(NoteType nType)
    {
        noteType = nType;
        justTime = Time.time + OptionManager.NoteJustTime;
        judgeObject = transform.GetChild(0).gameObject;

        switch (nType)
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
        while (Time.time < justTime)
        {
            judgeObject.transform.localScale = (Vector3)Vector2.one * (OptionManager.NoteJustTime*(justTime - Time.time) + OptionManager.JustTimeJudgeSize);
            yield return new WaitForEndOfFrame();
        }

        Instantiate(judgeEffectPrefab, transform.position, Quaternion.identity).GetComponent<JudgeEffect>().Setup(Judge.Perfact);
        Destroy(this.gameObject);
    }
}
