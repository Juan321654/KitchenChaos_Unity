using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateFryingOrFried += StoveCounter_OnStateFryingOrFried;
    }

    private void StoveCounter_OnStateFryingOrFried(object sender, EventArgs e)
    {
        bool playSound = stoveCounter.GetState() == StoveCounter.State.Frying || stoveCounter.GetState() == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
