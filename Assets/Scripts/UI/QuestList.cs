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

    void Update()
    {
        if (GameState.Current.Stage != prevStage)
            RecalculateState();
    }

    int prevStage;
    void RecalculateState()
    {
        prevStage = GameState.Current.Stage;
        foreach (var line in QuestLines)
            line.Striked = prevStage >= line.MinStage;
    }
}