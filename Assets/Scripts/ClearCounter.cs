using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    public GameObject prefabToShow;

    public void Interact()
    {
        Debug.Log("INTERACTED!");
    }

    public void Highlight(bool highlight)
    {
        if (prefabToShow == null) return;
        
        if (highlight)
        {
            prefabToShow.SetActive(true);
        }
        else
        {
            prefabToShow.SetActive(false);
        }
    }
}