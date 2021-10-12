using UnityEngine;

public class HitRange : MonoBehaviour
{
    public NoteType noteType { get; private set; }

    [SerializeField] Color rightSideColor;
    [SerializeField] Color leftSideColor;

    [SerializeField] Sprite handSprite;
    [SerializeField] Sprite elbowSprite;
    [SerializeField] Sprite kneeSprite;

    public void SetUp(NoteType nType)
    {
        noteType = nType;
        switch(nType)
        {
            case NoteType.HAND_LEFT:
            case NoteType.HAND_RIGHT:
                GetComponent<SpriteRenderer>().sprite = handSprite;
                break;
            case NoteType.ELBOW_LEFT:
            case NoteType.ELBOW_RIGHT:
                GetComponent<SpriteRenderer>().sprite = elbowSprite;
                break;
            case NoteType.KNEE_LEFT:
            case NoteType.KNEE_RIGHT:
                GetComponent<SpriteRenderer>().sprite = kneeSprite;
                break;
            default:
                break;
        }

        switch (nType)
        {
            case NoteType.HAND_LEFT:
            case NoteType.ELBOW_LEFT:
            case NoteType.KNEE_LEFT:
                GetComponent<SpriteRenderer>().color = leftSideColor;
                break;
            default:
                GetComponent<SpriteRenderer>().color = rightSideColor;
                break;
        }
    }
}
