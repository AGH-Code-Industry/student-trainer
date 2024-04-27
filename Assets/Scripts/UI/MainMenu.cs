using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject PlayBtn;
    [SerializeField] private GameObject SettingsBtn;
    [SerializeField] private GameObject QuitBtn;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject menuCam;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menuPanel;
    private void Awake()
    {
        UIEvents.ClickPlay += PlayGame;
        UIEvents.ClickSettings += Settings;
        UIEvents.ClickQuit += Quit;
        UIEvents.ClickBack += Back;
        settingsPanel.SetActive(false);
    }

    private void OnDisable()
    {
        UIEvents.ClickPlay -= PlayGame;
        UIEvents.ClickSettings -= Settings;
        UIEvents.ClickQuit -= Quit;
        UIEvents.ClickBack -= Back;
    }

    public void Back()
    {
        menuCam.GetComponent<Animator>().Play("settings_tran");
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void PlayGame()
    {
        Debug.Log("PlayGame btn clicked!");
        mainCam.SetActive(true);
        menuCam.SetActive(false);
        this.gameObject.SetActive(false);
        InputManager.Instance.GetInput().Enable();
    }

    public void Settings()
    {
        Debug.Log("Settings btn clicked!");
        menuCam.GetComponent<Animator>().Play("menu_tran");
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
        /*PlayBtn.SetActive(false);
        SettingsBtn.SetActive(false);
        QuitBtn.SetActive(false);*/
    }

    public void Quit()
    {
        Debug.Log("Quit btn clicked!");
        Application.Quit();
    }
}