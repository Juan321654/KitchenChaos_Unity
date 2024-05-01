using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<OnProgressChangedEventArgs> OnCuttingProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
    public event EventHandler OnCut;


    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
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

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
            cuttingProgress += 1;

            OnCut?.Invoke(this, EventArgs.Empty);

            OnCuttingProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                CutKitchenObject();
            }

        }
    }

    private void CutKitchenObject()
    {
        KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

        GetKitchenObject().DestroySelf();

        KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
    }

    private void TransferKitchenObjectToPlayer(Player player)
    {
        if (!player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
        }
    }

    private void TransferKitchenObjectFromPlayer(Player player)
    {
        if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            cuttingProgress = 0;

            OnCuttingProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { progressNormalized = 0 });
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }

        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }

        return null;
    }
}
