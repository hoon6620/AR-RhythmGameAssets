using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JudgeEffect : MonoBehaviour
{
    [SerializeField]
    float fadeTime;

    TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(Judge judge)
    {
        switch(judge)
        {
            case Judge.Perfact:
                textMesh.text = "Perfact";
                textMesh.faceColor = Color.green;
                break;

            case Judge.Good:
                textMesh.text = "Good";
                textMesh.faceColor = Color.yellow;
                break;

            case Judge.Bad:
                textMesh.text = "Bad";
                textMesh.faceColor = Color.red;
                break;

            case Judge.Miss:
                textMesh.text = "Miss";
                textMesh.faceColor = Color.gray;
                break;
        }
        StartCoroutine(EffectCo());
    }

    public IEnumerator EffectCo()
    {
        float endTime = Time.time + fadeTime;
        while (Time.time < endTime)
        {
            textMesh.alpha -= (Time.deltaTime / fadeTime);
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
}
