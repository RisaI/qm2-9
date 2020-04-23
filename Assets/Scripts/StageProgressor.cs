using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageProgressor : MonoBehaviour
{
    [Range(0, 10)]
    public int Stage;

    public bool RemoveOnProgress;

    void Start()
    {
        if (RemoveOnProgress && GameState.Current.Stage >= Stage)
            gameObject.SetActive(false);
    }
}
