using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] protected GameObject prefabToShowOnHighlight;
    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject;
    public virtual void Interact(Player player)
    {
        Debug.LogError("Interacting with BaseCounter class should not be possible. Please check the derived class.");
    }
    public virtual void InteractAlternate(Player player)
    {
        Debug.LogError("Interacting ALT with BaseCounter class should not be possible. Please check the derived class.");
    }

    public void Highlight(bool highlight)
    {
        if (prefabToShowOnHighlight == null) return;

        prefabToShowOnHighlight.SetActive(highlight);
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
