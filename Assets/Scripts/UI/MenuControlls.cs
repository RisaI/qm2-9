using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControlls : MonoBehaviour
{
    const string DriveURL = "http://www.pavelstransky.cz/", GithubURL = "https://github.com/RisaI/qm2-9";

    public UnityEngine.UI.Image LockImage;
    public GameObject LoadingText, ContinueButton, Congrats;

    public GameObject[] MenuStates;

    // Start is called before the first frame update
    void Start()
    {
        LockImage.enabled = !Settings.Current.Finished;
        ContinueButton.SetActive(System.IO.File.Exists(GameState.FileName));
        LoadingText.SetActive(false);
        Congrats.SetActive(Settings.Current.Finished);

        RecoverMenuState();
    }

    AsyncOperation CurrentLoad;

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        if (CurrentLoad != null)
            LoadingText.GetComponent<TMPro.TextMeshProUGUI>().text = $"Kvantuji... ({(CurrentLoad.progress * 100).ToString("N0")}%)";
    }

    public void SetMenuState(int i)
    {
        if (i < 0 || i >= MenuStates.Length)
            return;

        Settings.Current.MenuState = i;
        RecoverMenuState();
    }

    void RecoverMenuState()
    {
        for (int i = 0; i < MenuStates.Length; ++i)
            MenuStates[i].SetActive(false);

        MenuStates[Settings.Current.MenuState].SetActive(true);
    }

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

    public void OnDriveClicked()
    {
        if (Settings.Current.Finished)
        {
            Application.OpenURL(DriveURL);
        }
    }

    public void OnGithubClicked()
    {
        Application.OpenURL(GithubURL);
    }

    public void Exit()
    {
        if (CurrentLoad == null)
            Application.Quit();
    }
}
