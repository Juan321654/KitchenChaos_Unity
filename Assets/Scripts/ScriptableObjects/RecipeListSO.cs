using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu()] // This attribute is commented out to prevent the creation of RecipeListSO assets in the project folder as only one instance of RecipeListSO is needed
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> recipeSOList;
}
