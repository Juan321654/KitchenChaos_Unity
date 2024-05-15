using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefSO audioClipRefSO;
    [SerializeField] private float soundVolumen = 0.2f;
    private Vector3 cameraPosition;

    private void Awake()
    {
        cameraPosition = Camera.main.transform.position;
        Instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        PlaySound(audioClipRefSO.trash, cameraPosition, soundVolumen);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        PlaySound(audioClipRefSO.objectDrop, cameraPosition, soundVolumen);
    }

    private void Player_OnPickedSomething(object sender, EventArgs e)
    {
        PlaySound(audioClipRefSO.objectPickup, cameraPosition, soundVolumen);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        PlaySound(audioClipRefSO.chop, cameraPosition, soundVolumen);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        // DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefSO.deliveryFail, cameraPosition, soundVolumen);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        // DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        // PlaySound(audioClipRefSO.deliverySuccess, deliveryCounter.transform.position);
        PlaySound(audioClipRefSO.deliverySuccess, cameraPosition, soundVolumen);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        if (audioClipArray.Length > 0)
        {
            AudioClip audioClip = audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)];
            PlaySound(audioClip, position, volume);
        }
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefSO.footstep, position, volume);
    }
}
