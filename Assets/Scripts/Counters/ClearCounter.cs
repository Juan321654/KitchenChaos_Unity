using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            TransferKitchenObjectToPlayer(player);
        }
        else
        {
            TransferKitchenObjectFromPlayer(player);
        }
    }


    private void HandlePlateWithIngredient(Player player)
    {
        if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) // player holding a plate
        {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) // add the ingredient to the plate
            {
                GetKitchenObject().DestroySelf();
            }
        }
        else
        {
            if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) // player not holding a plate but the counter has a plate
            {
                if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) // add the ingredient to the plate
                {
                    player.GetKitchenObject().DestroySelf();
                }
            }
        }
    }

    private void TransferKitchenObjectToPlayer(Player player)
    {
        if (!player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
        }
        else
        {
            HandlePlateWithIngredient(player);
        }
    }

    private void TransferKitchenObjectFromPlayer(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
    }
}
