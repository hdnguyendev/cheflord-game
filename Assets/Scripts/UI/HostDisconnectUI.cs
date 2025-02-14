using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectUI : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;

    private void Start() {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

        playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);

        Hide();
    }

    private void OnPlayAgainButtonClicked()
    {
        Loader.Load(Loader.Scene.MainMenuScene);
    }
    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId) {
            // Server is sutting down
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
    }
}
