using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut; // belongs to the class, not each instance
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;


    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            TransferKitchenObjectToPlayer(player);
            HandlePlateWithIngredient(player);
        }
        else
        {
            TransferKitchenObjectFromPlayer(player);
        }
    }

    private void HandlePlateWithIngredient(Player player)
    {
        if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().DestroySelf();
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
            cuttingProgress += 1;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });

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

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 });
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
