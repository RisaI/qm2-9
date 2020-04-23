using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public Vector3 _Direction = Vector3.zero;
    public Vector3 Direction { get { return _Direction != Vector3.zero ? _Direction : transform.up; } }
}
