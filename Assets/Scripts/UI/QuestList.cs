using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestList : MonoBehaviour
{
    public QuestLine[] QuestLines;

    // Start is called before the first frame update
    void Start()
    {
        RecalculateState();
    }

    void RecalculateState()
    {
        foreach (var line in QuestLines)
            line.Striked = GameState.Current.Stage >= line.MinStage;
    }
}