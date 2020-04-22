using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameState.Current == null)
            GameState.Current = GameState.LoadNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
