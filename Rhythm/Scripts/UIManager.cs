using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;
    void Awake() => Inst = this;

    [SerializeField]
    Text comboText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCombo(int combo)
    {
        comboText.text = combo.ToString();
    }
}
