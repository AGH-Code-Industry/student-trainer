/*

    From https://docs.unity3d.com/6000.1/Documentation/ScriptReference/SceneManagement.SceneManager.GetActiveScene.html
    "The currently active Scene is the Scene which will be used as the destination for new GameObjects and the source of current lighting settings."

    The current level scene should be the one that's active, at least with the current structure

*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine.Assertions;

public class SceneService
{
    List<string> loadedScenes = new List<string>();

    /*
    readonly string[] MENU_SCENES = { "Student City", "Menu" };
    readonly string[] PLAYER_SCENES = { "Player", "Player UI" };

    const string CITY = "Student City", KAPITOL = "LVL_1";
    const string SPAWN_CITY1 = "SP_FROM_MENU", SPAWN_CITY2 = "SP_FROM_KAPITOL", SPAWN_KAPITOL = "SP_FROM_CITY";
    */

    public AsyncOperation LoadScene(string sceneName)
    {
        if (!loadedScenes.Contains(sceneName))
        {
            loadedScenes.Add(sceneName);
            return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            return null;
        }
    }

    public AsyncOperation UnloadScene(string sceneName)
    {
        if (loadedScenes.Contains(sceneName))
        {
            loadedScenes.Remove(sceneName);
            return SceneManager.UnloadSceneAsync(sceneName);
        }
        else
        {
            return null;
        }
    }
    
    public List<AsyncOperation> LoadSceneList(string[] scenes)
    {
        List<AsyncOperation> result = new List<AsyncOperation>();

        foreach(string scene in scenes)
        {
            AsyncOperation op = LoadScene(scene);
            if(op != null)
                result.Add(op);
        }

        return result;
    }

    public List<AsyncOperation> UnloadSceneList(string[] scenes)
    {
        List<AsyncOperation> result = new List<AsyncOperation>();

        foreach (string scene in scenes)
        {
            AsyncOperation op = UnloadScene(scene);
            if(op != null)
                result.Add(op);
        }

        return result;
    }
    
    public void SetActiveScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if(scene == null || !scene.IsValid())
        {
            Debug.LogError($"SceneService: could not find active scene \"{sceneName}\"!");
            return;
        }
        else if(!scene.isLoaded)
        {
            Debug.LogError($"SceneService: scene \"{sceneName}\" was found, but is not currenly loaded!");
            return;
        }

        Assert.IsTrue(loadedScenes.Contains(sceneName), "SceneService: scene's isLoaded property is true, but it's not in the list of loaded scenes!");

        SceneManager.SetActiveScene(scene);
    }
}
