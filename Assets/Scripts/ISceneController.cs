
using UnityEngine;

interface ISceneController
{
    (Vector3, Quaternion) GetCheckPoint(int stage);
}