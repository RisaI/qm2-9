using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointNotifier : MonoBehaviour
{
    private bool _show;

    private int _highest;
    
    // Start is called before the first frame update
    void Start()
    {
        _show = false;
        int _highest = 0;
        GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
        
        GameState.Current.OnCheckpointReach += (e) =>
        {
            if (e.Notify && !_show && (e.Index > _highest || e.Overwrite))
            {
                _highest = e.Index;
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
