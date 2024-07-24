using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance { get; private set; }
    [SerializeField] private Button playButton;
    [SerializeField] private Button playSingleButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button optionsButton;


    public EventHandler OnClickOptionsButton;

    public void Awake()
    {
        Instance = this;
        playButton.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.playMultiplayer = true;
            Loader.Load(Loader.Scene.LobbyScene);
        });

        playSingleButton.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.playMultiplayer = false;
            Loader.Load(Loader.Scene.LobbyScene);
        });

        quitButton.onClick.AddListener(OnQuitButtonClicked);
        optionsButton.onClick.AddListener(OnOptionsButtonClicked);

        Time.timeScale = 1f;
    }

    private void OnOptionsButtonClicked()
    {
        OnClickOptionsButton?.Invoke(this, EventArgs.Empty);
    }
    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }


}
