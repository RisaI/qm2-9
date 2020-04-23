using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStul : MonoBehaviour
{
    
    public Komparator komparator;
    public FinalSpektrum skript;
    
    public void TriggerFinal()
    {
        komparator.ChangeText();
        skript.ShowStuff();
        StartCoroutine(ShowText());

    }
    
    private IEnumerator ShowText()
    {
        yield return new WaitForSeconds(10);
        //TODO: UKONČIT HRU
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
