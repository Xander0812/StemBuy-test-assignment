using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LostScreen;

    public static Action ActivateWinScreenAction = default;
    public static Action ActivateLostScreenAction = default;

    public void Awake()
    {
        restartButton.onClick.AddListener(ResetButtonClicked);
        ActivateWinScreenAction += ActivateWinScreen;
        ActivateLostScreenAction += ActivateLostScreen;
    }

    public void ActivateWinScreen()
    {
        WinScreen.SetActive(true);
        restartButton.gameObject.SetActive(false);
    }

     public void ActivateLostScreen()
    {
        LostScreen.SetActive(true);
    }


    private void ResetButtonClicked()
    {
        AudioManager.PlayButtonClickAction.Invoke();

        LostScreen.SetActive(false);
        FigureSpawner.ResetShapesAction.Invoke();
        MainGameplayManager.ResetAllTaskSlotsAction.Invoke();
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveAllListeners();
        ActivateWinScreenAction -= ActivateWinScreen;
        ActivateLostScreenAction -= ActivateLostScreen;
    }
}
