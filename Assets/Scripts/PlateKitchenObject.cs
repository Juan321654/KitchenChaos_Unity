using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public EventHandler<OnIngregientAddedEventArgs> OnIngredientAdded;
    public class OnIngregientAddedEventArgs : System.EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {  // not a valid ingredient
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngregientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });

            return true;
        }
    }
}
