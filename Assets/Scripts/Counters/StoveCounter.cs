using JetBrains.Annotations;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    [SerializeField] private StoveCounterVisual stoveCounterVisual;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

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
            fryingRecipeSO = GetFryingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
            burningRecipeSO = GetBurningRecipeSOForInput(fryingRecipeSO.output);
            state = State.Frying;
            fryingTimer = fryingRecipeSO.fryingTimerMax;
            stoveCounterVisual.SetStoveOn(true);
        }
    }

    private void Update()
    {
        if (!HasKitchenObject()) return;

        UpdateFryingState();
        UpdateBurningState();

    }

    private void UpdateFryingState()
    {
        if (state != State.Frying) return;

        fryingTimer -= Time.deltaTime;
        if (fryingTimer <= 0)
        {
            state = State.Fried;
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

            burningTimer = burningRecipeSO.burningTimerMax;
        }

    }

    private void UpdateBurningState()
    {
        if (state != State.Fried) return;

        burningTimer -= Time.deltaTime;
        if (burningTimer <= 0)
        {
            state = State.Burned;
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
        }

    }


    private FryingRecipeSO GetFryingRecipeSOForInput(KitchenObjectSO input)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == input)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOForInput(KitchenObjectSO input)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == input)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        return GetFryingRecipeSOForInput(input) != null;
    }

    private void TransferKitchenObjectToPlayer(Player player)
    {
        if (!player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            stoveCounterVisual.SetStoveOn(false);
            state = State.Idle;
            fryingTimer = fryingRecipeSO.fryingTimerMax;
            burningTimer = burningRecipeSO.burningTimerMax;
        }
    }

    private void TransferKitchenObjectFromPlayer(Player player)
    {
        if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
    }
}
