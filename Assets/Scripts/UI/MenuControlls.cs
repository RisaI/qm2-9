using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControlls : MonoBehaviour
{
    public UnityEngine.UI.Image LockImage;
    public GameObject LoadingText, ContinueButton;

    // Start is called before the first frame update
    void Start()
    {
        LockImage.enabled = !Settings.Current.Finished;
        ContinueButton.SetActive(System.IO.File.Exists(GameState.FileName));
        LoadingText.SetActive(false);
    }

    AsyncOperation CurrentLoad;

    public void StartGame()
    {
        LoadingText.SetActive(true);
        GameState.Current = GameState.LoadNewGame();
        CurrentLoad = SceneManager.LoadSceneAsync("Scenes/Game");
    }

    public void Continue()
    {
        if (!System.IO.File.Exists(GameState.FileName))
            return;
        
        LoadingText.SetActive(true);
        GameState.Current = GameState.LoadFromFile();
        CurrentLoad = SceneManager.LoadSceneAsync("Scenes/Game");
    }

    void Update()
    {
        if (CurrentLoad != null)
            LoadingText.GetComponent<TMPro.TextMeshProUGUI>().text = $"Kvantuji... ({(CurrentLoad.progress * 100).ToString("N0")}%)";
    }

    public void OnDriveClicked()
    {
        if (Settings.Current.Finished)
        {
            Application.OpenURL("https://www.seznam.cz/");
        }
    }

    public void Exit()
    {
        if (CurrentLoad == null)
            Application.Quit();
    }
}
