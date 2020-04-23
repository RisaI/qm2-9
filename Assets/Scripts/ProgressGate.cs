using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressGate : MonoBehaviour
{
    public Collider Gate;
    public TMPro.TextMeshPro Text;

    // Update is called once per frame
    void Update()
    {
        Gate.enabled = Text.enabled = GameState.Current.Stage < 3;
    }
}
