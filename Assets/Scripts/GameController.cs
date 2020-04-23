using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour, ISceneController
{
    void Awake()
    {
        if (GameState.Current == null)
            GameState.Current = GameState.LoadNewGame();

        GameState.Current.Stage = 3;
        GameState.Current.CheckpointIndex = 7;
    }
    
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Death();
    }

    public (Vector3, Quaternion) GetCheckPoint(int idx)
    {
        var checkpoints = GameObject.FindGameObjectsWithTag("Respawn");

        var point = checkpoints.FirstOrDefault(c => c.GetComponent<Checkpoint>()?.Index == idx) ?? checkpoints.FirstOrDefault();
        return (point.transform.position, point.transform.rotation);
    }
}
