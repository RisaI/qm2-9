using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugController : MonoBehaviour, ISceneController
{
    // Start is called before the first frame update
    void Awake()
    {
        if (GameState.Current == null)
            GameState.Current = GameState.LoadNewGame();

        GameState.Current.Stage = 1;
        GameState.Current.CheckpointIndex = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public (Vector3, Quaternion) GetCheckPoint(int idx)
    {
        var checkpoints = GameObject.FindGameObjectsWithTag("Respawn");

        var point = checkpoints.FirstOrDefault(c => c.GetComponent<Checkpoint>()?.Index == idx) ?? checkpoints.FirstOrDefault();
        return (point.transform.position, point.transform.rotation);
    }

}
