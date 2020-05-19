using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalStul : MonoBehaviour
{
    
    public Komparator komparator;
    public FinalSpektrum skript;
    
    public void TriggerFinal()
    {
        komparator.ChangeText();
        skript.ShowStuff();
        StartCoroutine(FinishGame());
    }

    private IEnumerator FinishGame()
    {
        Settings.Current.Finished = true;
        Settings.Current.MenuState = 1; // Show credits
        Settings.Current.SaveToFile();

        yield return new WaitForSeconds(10);
        
        SceneManager.LoadScene("Scenes/Menu");
    }
}
