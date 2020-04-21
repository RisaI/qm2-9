using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControlls : MonoBehaviour
{
    public bool DriveLocked;
    public UnityEngine.UI.Image DriveImage;

    // Start is called before the first frame update
    void Start()
    {
        DriveImage.enabled = DriveLocked;
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void OnDriveClicked()
    {
        if (!DriveLocked)
        {
            Application.OpenURL("https://www.seznam.cz/");
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
