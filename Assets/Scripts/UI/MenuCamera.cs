using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [Range(-60, 60)] public float YSpeed;
    [Range(-60, 60)] public float ZSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float yAngle;
    float zAngle;

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, yAngle = yAngle + Time.deltaTime * YSpeed, zAngle = zAngle + Time.deltaTime * ZSpeed);
    }
}
