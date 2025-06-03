using UnityEngine;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject PlayBtn;
    [SerializeField] private GameObject SettingsBtn;
    [SerializeField] private GameObject QuitBtn;
    [SerializeField] private GameObject menuCam;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menuPanel;

    [Inject] private readonly LevelService levelService;

    private void Awake()
    {
        settingsPanel.SetActive(false);
    }

    public void Back()
    {
        menuCam.GetComponent<Animator>().Play("settings_tran");
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void PlayGame()
    {
        levelService.LoadGame();
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