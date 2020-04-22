using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour, ISceneController
{
    public GameObject SpawnPoint;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameState.Current == null)
            GameState.Current = GameState.LoadNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public (Vector3, Quaternion) GetCheckPoint(int idx)
    {
        return (SpawnPoint.transform.position, SpawnPoint.transform.rotation);
    }

}
