using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLine : MonoBehaviour
{
    [TextArea(1,3)] public string Text;
    [TextArea(1,3)] public string StrikedText;

    [Range(0, 10)]
    public int MinStage;

    public bool _Striked;
    public bool Striked {
        get { return _Striked; }
        set {
            _Striked = value;
            SetText();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    void SetText()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = _Striked ? StrikedText : Text;
    }
}
