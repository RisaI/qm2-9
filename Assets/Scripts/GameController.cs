using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControlelr : MonoBehaviour, ISceneController
{
    public (Vector3, Quaternion) GetCheckPoint(int idx)
    {
        var checkpoints = GameObject.FindGameObjectsWithTag("Respawn");

        var point = checkpoints.FirstOrDefault(c => c.GetComponent<Checkpoint>()?.Index == idx) ?? checkpoints.FirstOrDefault();
        return (point.transform.position, point.transform.rotation);
    }
}
