using UnityEngine;

public class BaseCounter : MonoBehaviour
{
    [SerializeField] protected GameObject prefabToShowOnHighlight;
    public virtual void Interact(Player player)
    {
        Debug.LogError("Interacting with BaseCounter class should not be possible. Please check the derived class.");
    }

    public void Highlight(bool highlight)
    {
        if (prefabToShowOnHighlight == null) return;

        prefabToShowOnHighlight.SetActive(highlight);
    }
}
