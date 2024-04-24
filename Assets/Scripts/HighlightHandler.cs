using UnityEngine;

public class HighlightHandler : MonoBehaviour
{
    public static void Highlight(GameObject prefabToShow, bool highlight)
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
