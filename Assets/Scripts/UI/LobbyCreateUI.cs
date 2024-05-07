using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;


    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        createPrivateButton.onClick.AddListener(() =>
        {
            string lobbyName = lobbyNameInputField.text;

            KitchenGameLobby.Instance.CreateLobby(lobbyName ??= "LobbyName", true);
        });
        createPublicButton.onClick.AddListener(() =>
        {
            string lobbyName = lobbyNameInputField.text;

            KitchenGameLobby.Instance.CreateLobby(lobbyName ??= "LobbyName", false);
        });
    }
    private void Start() {
        Hide();
    }
    private void Hide() {
            gameObject.SetActive(false);

    }

    public void Show() {
        gameObject.SetActive(true);
    }
}
