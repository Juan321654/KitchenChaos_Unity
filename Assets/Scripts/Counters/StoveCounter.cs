using System;
using JetBrains.Annotations;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnStateFryingOrFried;
    public enum State
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

    public State GetState()
    {
        return state;
    }

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

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            fryingRecipeSO = GetFryingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
            burningRecipeSO = GetBurningRecipeSOForInput(fryingRecipeSO.output);
            state = State.Frying;
            fryingTimer = 0;
            stoveCounterVisual.SetStoveOn(true);
            OnStateFryingOrFried?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandlePlateWithIngredient(Player player)
    {
        if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().DestroySelf();
                stoveCounterVisual.SetStoveOn(false);
                state = State.Idle;
                fryingTimer = 0;
                burningTimer = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 });
                OnStateFryingOrFried?.Invoke(this, EventArgs.Empty);
            }
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

        fryingTimer += Time.deltaTime;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
        if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
        {
            state = State.Fried;
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

            burningTimer = 0;
            OnStateFryingOrFried?.Invoke(this, EventArgs.Empty);
        }

    }

    private void UpdateBurningState()
    {
        if (state != State.Fried) return;

        burningTimer += Time.deltaTime;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = burningTimer / burningRecipeSO.burningTimerMax });
        if (burningTimer >= burningRecipeSO.burningTimerMax)
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
            fryingTimer = 0;
            burningTimer = 0;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 });
            OnStateFryingOrFried?.Invoke(this, EventArgs.Empty);
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
