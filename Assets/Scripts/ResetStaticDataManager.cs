using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    // classes with static data don't need to be instantiated
    // but we need to reset the static data when the game starts
    // because the static data persists between play mode sessions
    // static data belongs to the class, not to the instance
    
    private void Awake()
    {
        CuttingCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
    }
}
