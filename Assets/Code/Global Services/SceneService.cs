using UnityEngine.SceneManagement;

public class SceneService
{
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    public void LoadAdditiveScene(string sceneName) => SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

    public void LoadMenu()
    {
        LoadScene("Menu");
        LoadAdditiveScene("Student City");
    }

    public void LoadGame()
    {
        LoadScene("Player");
        LoadAdditiveScene("Player UI");
        LoadAdditiveScene("Student City");
    }
}
