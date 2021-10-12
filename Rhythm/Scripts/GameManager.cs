using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static int combo;
    static Dictionary<Judge, int> judgeCount;
    // Start is called before the first frame update
    void Start()
    {
        GameSetup();
    }

    static void GameSetup()
    {
        combo = 0;
        judgeCount = new Dictionary<Judge, int>();
        judgeCount.Add(Judge.Perfact, 0);
        judgeCount.Add(Judge.Good, 0);
        judgeCount.Add(Judge.Bad, 0);
        judgeCount.Add(Judge.Miss, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public void NoteJudge(Judge judge)
    {
        judgeCount[judge]++;
        if (judge <= Judge.Good)
            combo++;
        else
            combo = 0;
        UIManager.Inst.SetCombo(combo);
    }
}
