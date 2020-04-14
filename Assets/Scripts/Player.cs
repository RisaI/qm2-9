﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController Controller;
    
    public Camera Camera;

    [Range(.0f, 1f)]
    public float Speed = 0.1f;

    public Vector3 HorizontalForward
    {
        get { return Vector3.Scale(Controller.transform.forward, new Vector3(1,0,1)).normalized; }
    }

    public Vector3 HorizontalRight
    {
        get {
            var fwd = HorizontalForward;
            return new Vector3(fwd.z, 0, -fwd.x);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
    }

    float cameraRotation = 0;
    // Update is called once per frame
    void Update()
    {
        var verInput = Input.GetAxis("Vertical");
        var horInput = Input.GetAxis("Horizontal");
        
        var recSize = Mathf.Cos(Mod(Mathf.Atan2(verInput, horInput) + Mathf.PI / 4, Mathf.PI / 2) - Mathf.PI / 4f);

        Controller.Move(verInput * Speed * HorizontalForward * recSize);
        Controller.Move(horInput * Speed * HorizontalRight * recSize);
        
        Cursor.lockState = CursorLockMode.Locked;
        var mouseX = Input.GetAxisRaw("Mouse X");
        var mouseY = Input.GetAxisRaw("Mouse Y");

        Controller.transform.Rotate(0, mouseX, 0);
        Camera.transform.localRotation = Quaternion.Euler(cameraRotation = Mathf.Clamp(cameraRotation - mouseY, -90, 90), 0, 0);
        // Debug.Log(mouseY);
    }

    public float Mod(float lhs, float rhs)
    {
        return (lhs % rhs + rhs) % rhs;
    }
}