using UnityEngine;
using TMPro;
using Zenject;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] GameObject canvasObj, eventSysObj;
    [SerializeField] TextMeshProUGUI loadingText;

    [Inject] readonly EventBus eventBus;

    void Start()
    {
        eventBus.Subscribe<LevelChangeBegin>(ShowLoadingScreen);
        eventBus.Subscribe<LevelChangeFinish>(HideLoadingScreen);
    }

    void Update()
    {
        
    }

    void ShowLoadingScreen(LevelChangeBegin beginEvent)
    {
        canvasObj.SetActive(true);
        eventSysObj.SetActive(true);

        loadingText.text = "Ładowanie: " + beginEvent.levelName;
    }

    void HideLoadingScreen(LevelChangeFinish finishEvent)
    {
        canvasObj.SetActive(false);
        eventSysObj.SetActive(false);
    }


    private void OnDestroy()
    {
        eventBus.Unsubscribe<LevelChangeBegin>(ShowLoadingScreen);
        eventBus.Unsubscribe<LevelChangeFinish>(HideLoadingScreen);
    }
}