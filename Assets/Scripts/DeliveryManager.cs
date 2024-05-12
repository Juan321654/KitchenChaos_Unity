using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            SpawnRecipe();
        }

    }

    private void SpawnRecipe()
    {
        if (waitingRecipeSOList.Count < waitingRecipesMax)
        {
            RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
            Debug.Log("Spawning recipe: " + waitingRecipeSO.recipeName);
            waitingRecipeSOList.Add(waitingRecipeSO);
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool isRecipeMatch = true;
                for (int j = 0; j < waitingRecipeSO.kitchenObjectSOList.Count; j++)
                {
                    KitchenObjectSO waitingKitchenObjectSO = waitingRecipeSO.kitchenObjectSOList[j];
                    KitchenObjectSO plateKitchenObjectSO = plateKitchenObject.GetKitchenObjectSOList()[j];
                    if (waitingKitchenObjectSO != plateKitchenObjectSO)
                    {
                        isRecipeMatch = false;
                        break;
                    }
                }
                if (isRecipeMatch)
                {
                    Debug.Log("Recipe delivered: " + waitingRecipeSO.recipeName);
                    waitingRecipeSOList.RemoveAt(i);
                    Destroy(plateKitchenObject.gameObject);
                    break;
                }
            }
        }

        Debug.Log("Bad recipe delivered!");
    }
}
