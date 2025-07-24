using UnityEngine;
using System.Collections;

public class DeathScreen : WindowUI
{
    public override string dataID { get; protected set; } = "DeathScreen";
    public GameObject panel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    public override void OnWindowOpened()
    {
        StartCoroutine(ShowCoroutine());
    }

    public override void OnWindowClosed()
    {
        return;
    }

    IEnumerator ShowCoroutine()
    {
        yield return new WaitForSecondsRealtime(2f);
        panel.SetActive(true);
    }
}
