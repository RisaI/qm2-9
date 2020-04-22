using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController Controller;
    
    public Camera Camera;

    [Range(0, 100)]
    public float Speed = 1f;

    public Vector3 HorizontalForward
    {
        get {
            return Controller.transform.forward;
        }
    }

    public Vector3 HorizontalRight
    {
        get {
            return Controller.transform.right;
        }
    }

    public Vector3 Up => Controller.transform.up;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
    }

    float cameraRotation = 0;
    Vector3 velocity = Vector3.zero;

    bool restartAvailable;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Restart") > 0.5f)
        {
            if (restartAvailable)
                Death();

            restartAvailable = false;
            return;
        }
        else
        {
            restartAvailable = true;
        }

        var verInput = Input.GetAxis("Vertical");
        var horInput = Input.GetAxis("Horizontal");
        
        var recSize = Mathf.Cos(Mod(Mathf.Atan2(verInput, horInput) + Mathf.PI / 4, Mathf.PI / 2) - Mathf.PI / 4f);

        // Controller.Move(verInput * Speed * HorizontalForward * recSize);
        Controller.Move(
            ((verInput * HorizontalForward + horInput  * HorizontalRight) * Speed * recSize + velocity) * Time.deltaTime
        );
        
        Cursor.lockState = CursorLockMode.Locked;
        var mouseX = Input.GetAxisRaw("Mouse X");
        var mouseY = Input.GetAxisRaw("Mouse Y");

        Controller.transform.Rotate(0, mouseX, 0);
        Camera.transform.localRotation = Quaternion.Euler(cameraRotation = Mathf.Clamp(cameraRotation - mouseY, -90, 90), 0, 0);
        
        if (Physics.Raycast(transform.position, -Controller.transform.up, 1.1f))
        {
            velocity = -Up * 1e-1f;
            if (Input.GetAxis("Jump") > 0.5f)
            {
                Debug.Log("jump");
                velocity = Up * 4;
                Controller.Move(velocity * Time.deltaTime);
            }
        }
        else
        {
            velocity += Up * Time.deltaTime * Physics.gravity.y;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Checkpoint check;
        if (other.TryGetComponent<Checkpoint>(out check))
        {
            GameState.Current.CheckpointIndex = check.Index;

            if (GameState.Current.Stage < check.Stage)
                GameState.Current.Stage = check.Stage;

            if (GameState.Current.Dirty)
                GameState.Current.SaveToFile();
        }

        Flipper flip;
        if (other.TryGetComponent<Flipper>(out flip))
        {
            Flip(-flip.transform.up);
        }
    }

    public void Death()
    {
        var (pos, rot) = this.gameObject.scene.GetRootGameObjects()
            .First(go => go.GetComponent<ISceneController>() != null)
            .GetComponent<ISceneController>()
            .GetCheckPoint(GameState.Current.Stage);

        velocity = Vector3.zero;
        Controller.transform.position = pos;
        Controller.transform.rotation = rot;
    }

    public void Flip(Vector3 newUp)
    {
        Controller.transform.rotation = Quaternion.LookRotation(HorizontalForward - newUp * Vector3.Dot(newUp, HorizontalForward), newUp);
    }

    public float Mod(float lhs, float rhs)
    {
        return (lhs % rhs + rhs) % rhs;
    }
}