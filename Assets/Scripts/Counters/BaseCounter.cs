using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event System.EventHandler OnAnyObjectPlacedHere;
    [SerializeField] protected GameObject prefabToShowOnHighlight;
    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject;
    public virtual void Interact(Player player)
    {
        Debug.Log("Interacting with BaseCounter class should not be possible. Please check the derived class.");
    }
    public virtual void InteractAlternate(Player player)
    {
        Debug.Log("Interacting ALT with BaseCounter class should not be possible. Please check the derived class.");
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

        if (kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, System.EventArgs.Empty);
        }
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
