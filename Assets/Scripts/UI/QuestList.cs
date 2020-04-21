using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestList : MonoBehaviour
{
    public QuestLine[] QuestLines;

    [Range(0, 10)]
    public int _Stage;
    public int Stage
    {
        get { return _Stage; }
        set {
            _Stage = value;
            RecalculateState();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RecalculateState();
    }

    void RecalculateState()
    {
        foreach (var line in QuestLines)
            line.Striked = _Stage >= line.MinStage;
    }
}