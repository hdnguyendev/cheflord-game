using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;
    public void Awake()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        optionsButton.onClick.AddListener(OnOptionsButtonClicked);
    }

    private void OnOptionsButtonClicked()
    {
        OptionsUI.Instance.Show();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnLocalPauseAction += Instance_OnLocalPauseAction;
        KitchenGameManager.Instance.OnLocalContinueAction += Instance_OnLocalContinueAction;

        Hide();
    }

    private void OnMainMenuButtonClicked()
    {
        KitchenGameManager.Instance.TogglePauseGame();
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    private void OnResumeButtonClicked()
    {
        KitchenGameManager.Instance.TogglePauseGame();
    }


    private void Instance_OnLocalContinueAction(object sender, EventArgs e)
    {
        Hide();
    }

    private void Instance_OnLocalPauseAction(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
