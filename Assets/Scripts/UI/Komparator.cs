using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Komparator : MonoBehaviour
{

    public void ChangeText()
    {
        GetComponent<TMPro.TextMeshPro>().text = "STEJNÉ!";
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
