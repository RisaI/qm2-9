using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointNotifier : MonoBehaviour
{
    private bool _show;

    // Start is called before the first frame update
    void Start()
    {
        _show = false;
        GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
        
        GameState.Current.OnCheckpointReach += (e) =>
        {
            if (e.Notify && !_show)
            {
                Debug.Log("Showing text");
                StartCoroutine(ShowText());
            }
        };
    }

    private IEnumerator ShowText()
    {
        _show = true;
        GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
        yield return new WaitForSeconds(3);
        GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
        _show = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
