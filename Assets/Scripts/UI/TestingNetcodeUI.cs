using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    private void Awake()
    {
        startHostButton.onClick.AddListener(StartHost);
        startClientButton.onClick.AddListener(StartClient);
    }

    private void StartClient()
    {
        Debug.Log("Starting client...");
        NetworkManager.Singleton.StartClient();
        Hide();
    }

    private void StartHost()
    {
        Debug.Log("Starting host...");
        NetworkManager.Singleton.StartHost();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
