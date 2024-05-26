using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefSO audioClipRefSO;
    private Vector3 cameraPosition;
    private float volume = 1f;
    private float defaultVolume = 1f;

    private void Awake()
    {
        cameraPosition = Camera.main.transform.position;
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, defaultVolume); // (defaultVolume) Set the default volume if it doesn't exist
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
        PlaySound(audioClipRefSO.trash, cameraPosition);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        PlaySound(audioClipRefSO.objectDrop, cameraPosition);
    }

    private void Player_OnPickedSomething(object sender, EventArgs e)
    {
        PlaySound(audioClipRefSO.objectPickup, cameraPosition);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        PlaySound(audioClipRefSO.chop, cameraPosition);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        // DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefSO.deliveryFail, cameraPosition);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        // DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        // PlaySound(audioClipRefSO.deliverySuccess, deliveryCounter.transform.position);
        PlaySound(audioClipRefSO.deliverySuccess, cameraPosition);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        if (audioClipArray.Length > 0)
        {
            AudioClip audioClip = audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)];
            PlaySound(audioClip, position, volume);
        }
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefSO.footstep, position, volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefSO.warning, Vector3.zero);
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
