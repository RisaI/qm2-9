using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControlls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
