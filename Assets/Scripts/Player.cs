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

    public float HeightDamageTreshold = 20f;

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

        if (Physics.Raycast(transform.position, -Controller.transform.up, 1.1f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            var relVel = Vector3.Dot(Controller.transform.up, velocity);

            if (-relVel > HeightDamageTreshold)
            {
                Death();
                return;
            }

            velocity = -Up * 1e-1f;
            if (GameState.Current.YFlipUnlocked && Input.GetAxis("YFlip") > 0.5f)
            {
                Flip(-Up);
            }
            else if (Input.GetAxis("Jump") > 0.5f)
            {
                velocity = Up * 4;
            }
        }
        else
        {
            velocity += Up * Time.deltaTime * Physics.gravity.y;
        }

        Controller.Move(
            ((verInput * HorizontalForward + horInput  * HorizontalRight) * Speed * recSize + velocity) * Time.deltaTime
        );

        Cursor.lockState = CursorLockMode.Locked;
        var mouseX = Input.GetAxisRaw("Mouse X");
        var mouseY = Input.GetAxisRaw("Mouse Y");
        Controller.transform.Rotate(0, mouseX, 0);
        Camera.transform.localRotation = Quaternion.Euler(cameraRotation = Mathf.Clamp(cameraRotation - mouseY, -90, 90), 0, 0);
    }

    bool _died;
    void LateUpdate()
    {
        if (_died)
        {
            var (pos, rot) = this.gameObject.scene.GetRootGameObjects()
                .First(go => go.GetComponent<ISceneController>() != null)
                .GetComponent<ISceneController>()
                .GetCheckPoint(GameState.Current.CheckpointIndex);

            velocity = Vector3.zero;
            Camera.transform.localRotation = Quaternion.Euler(cameraRotation = 0, 0, 0);
            Controller.transform.position = pos;
            Controller.transform.rotation = rot;

            _died = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"Triggered by {other.gameObject.name}");
        StageProgressor prog;
        if (other.TryGetComponent<StageProgressor>(out prog))
        {
            Debug.Log($"Setting stage {prog.Stage}");
            if (GameState.Current.Stage < prog.Stage)
                GameState.Current.Stage = prog.Stage;

            if (prog.RemoveOnProgress)
                prog.gameObject.SetActive(false);
        }

        Checkpoint check;
        if (other.TryGetComponent<Checkpoint>(out check))
        {
            if (check.Index > GameState.Current.CheckpointIndex || check.OverwriteHigher)
                GameState.Current.CheckpointIndex = check.Index;

            if (GameState.Current.Dirty)
                GameState.Current.SaveToFile();
        }

        Flipper flip;
        if (other.TryGetComponent<Flipper>(out flip))
        {
            Flip(-flip.Direction);
        }
    }

    public void Death()
    {
        _died = true;
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