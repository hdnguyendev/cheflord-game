using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{

    public static TutorialUI Instance { get; private set; }
    private void Awake() {
        Instance = this;

    }
    private void Start()
    {
        KitchenGameManager.Instance.OnLocalPlayerReadyChanged += KitchenGameManager_OnLocalPlayerReadyChanged;
        Show();
    }

    private void KitchenGameManager_OnLocalPlayerReadyChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsLocalPlayerReady())
        {
            Hide();
        }   
    }

    public void Show() {
        gameObject.SetActive(true);
    }
    public void Hide() {
        gameObject.SetActive(false);
    }


}
