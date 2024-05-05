using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Start()
    {
        stoveOnGameObject.SetActive(false);
        particlesGameObject.SetActive(false);
    }

    public void SetStoveOn(bool isOn)
    {
        stoveOnGameObject.SetActive(isOn);
        particlesGameObject.SetActive(isOn);
    }
}
