using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public GameObject MiniMenu;

    // Start is called before the first frame update
    void Start()
    {
        GameState.Current.MiniMenuToggle += (state) => {
            MiniMenu.SetActive(state);
        };

        MiniMenu.SetActive(GameState.Current.MiniMenu);
    }

    bool _pressed;
    public void Update()
    {
        if (Input.GetAxis("Cancel") > 0.5f)
        {
            if (!_pressed)
            GameState.Current.MiniMenu = !GameState.Current.MiniMenu;

            _pressed = true;
        }
        else
            _pressed = false;
    }

    public void MainMenu()
    {
        if (GameState.Current?.Dirty ?? false)
            GameState.Current.SaveToFile();

        SceneManager.LoadScene("Scenes/Menu");
    }
}
