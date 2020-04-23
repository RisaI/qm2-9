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
    [Range(0, 20)]
    public float JumpVelocity = 4f;

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

    bool restartAvailable, wasOnGround;

    bool flipUnlocked;

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
        var relVel = Vector3.Dot(Up, velocity);

        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.05f, 0.5f), -Up, Controller.transform.rotation, 1.05f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            if (-relVel > HeightDamageTreshold && !wasOnGround)
            {
                Death();
                return;
            }
            else if (relVel <= 0)
            {
                wasOnGround = true;

                velocity = -Up * 10;
                if (GameState.Current.YFlipUnlocked && Input.GetAxis("YFlip") > 0.5f)
                {
                    Flip(-Up);
                }
                else if (Input.GetAxis("Jump") > 0.5f)
                {
                    wasOnGround = false;
                    velocity = Up * JumpVelocity;
                }
            }
            else
            {
                velocity += Up * Time.deltaTime * Physics.gravity.y;
                wasOnGround = false;
            }
        }
        else
        {
            if (wasOnGround ||
                (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.05f, 0.5f), Up, Controller.transform.rotation, 1.05f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore) && relVel > 0))
                velocity = Vector3.zero;
            // if (wasOnGround)
                // velocity -= Up * Vector3.Dot(Up, velocity);

            velocity += Up * Time.deltaTime * Physics.gravity.y;
            wasOnGround = false;
        }

        Controller.Move(
            ((verInput * HorizontalForward + horInput  * HorizontalRight) * Speed * recSize + velocity) * Time.deltaTime
        );

        if (GameState.Current.MiniMenu)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            var mouseX = Input.GetAxisRaw("Mouse X");
            var mouseY = Input.GetAxisRaw("Mouse Y");
            Controller.transform.Rotate(0, mouseX, 0);
            cameraRotation = Mathf.Clamp(cameraRotation - mouseY, -90, 90);
        }

        Camera.transform.localRotation = Quaternion.Euler(cameraRotation, 0, 0);
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
            Controller.transform.SetPositionAndRotation(pos, rot);

            _died = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"Triggered by {other.gameObject.name}");
        StageProgressor prog;
        if (other.TryGetComponent<StageProgressor>(out prog))
        {
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
            {
                GameState.Current.SaveToFile();
                GameState.Current.CheckpointReached(new CheckpointReachEventArgs(check.Index, true));
            }
        }

        Flipper flip;
        if (other.TryGetComponent<Flipper>(out flip))
        {
            Flip(-flip.Direction);
        }

        FinalStul finalstul;
        if (other.TryGetComponent<FinalStul>(out finalstul))
        {
            finalstul.TriggerFinal();
        }

        if (other.tag == "Killer")
            Death();
    }

    public void Death()
    {
        _died = true;
    }

    public void Flip(Vector3 newUp)
    {
        var camFwd = Camera.transform.forward;
        var camUp = Camera.transform.up;
        var camDot = Vector3.Dot(camFwd, newUp);

        Camera.transform.localRotation = Quaternion.Euler(cameraRotation = camDot >= 1f ? -90f : (camDot <= -1f ? 90f : Mathf.Acos(camDot) * 180 / Mathf.PI - 90), 0, 0);
    
        var lookVec = camFwd - newUp * Vector3.Dot(newUp, camFwd);

        if (lookVec == Vector3.zero)
            lookVec = -camUp + newUp * Vector3.Dot(newUp, -camUp);
        
        Controller.transform.rotation = Quaternion.LookRotation(lookVec, newUp);

        Debug.Log(newUp);
    }

    public float Mod(float lhs, float rhs)
    {
        return (lhs % rhs + rhs) % rhs;
    }
}