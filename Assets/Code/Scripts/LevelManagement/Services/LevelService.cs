using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

public class LevelService : IInitializable
{
    MonoBehaviour mediatorObj;
    string currentLevel;

    [Inject] readonly SceneService sceneService;
    [Inject] readonly ResourceReader reader;
    [Inject] readonly EventBus bus;

    GameLevels levelSettings;

    PlayerReference player;

    public void Initialize()
    {
        mediatorObj = Object.FindObjectOfType<LevelChanger>();
        if (mediatorObj == null)
        {
            // This warning should only show in dev builds
#if !UNITY_EDITOR
            Debug.LogError("LevelService: could not find the LevelChanger object, and thus cannot call the level change coroutine!");
#endif
            return;
        }

        levelSettings = reader.ReadSettings<GameLevels>();

        LoadMenu();
    }

    public void AssignMediator(MonoBehaviour newMediator)
    {
        mediatorObj = newMediator;
        LoadMenu();
    }

    public void LoadMenu()
    {
        mediatorObj.StartCoroutine(LoadMenuCoroutine());
    }

    private IEnumerator LoadMenuCoroutine()
    {
        bus.Publish(new LevelChangeBegin("Menu główne"));

        AsyncOperation[] ops = sceneService.LoadSceneList(levelSettings.menuScenes).ToArray();

        List<Scene> requiredScenes = new List<Scene>();

        foreach (string scene in levelSettings.menuScenes)
        {
            requiredScenes.Add(SceneManager.GetSceneByName(scene));
        }

        bool done = false;
        float progress = 0f;
        while (!done)
        {
            done = true;
            foreach (AsyncOperation op in ops)
            {
                if (!op.isDone)
                {
                    done = false;
                    break;
                }
            }

            foreach (AsyncOperation op in ops)
            {
                progress += op.progress;
            }

            foreach (Scene sc in requiredScenes)
            {
                if (!sc.IsValid() || !sc.isLoaded)
                    done = false;
            }

            progress /= ops.Length;

            bus.Publish(new LevelChangeProgress("Ładowanie sceny menu", progress, progress));

            yield return null;
        }

        //sceneService.SetActiveScene(levelSettings.menuScenes[0]);

        bus.Publish(new LevelChangeFinish());
    }

    public void LoadGame()
    {
        mediatorObj.StartCoroutine(LoadGameCoroutine());
    }

    private IEnumerator LoadGameCoroutine()
    {
        bus.Publish(new LevelChangeBegin("Gra"));

        List<AsyncOperation> ops = new List<AsyncOperation>();

        ops.AddRange(sceneService.UnloadSceneList(levelSettings.menuScenes));
        ops.AddRange(sceneService.LoadSceneList(levelSettings.playerScenes));
        ops.Add(sceneService.LoadScene(levelSettings.defaultLevel));

        List<Scene> requiredScenes = new List<Scene>();
        foreach (string scene in levelSettings.playerScenes)
        {
            requiredScenes.Add(SceneManager.GetSceneByName(scene));
        }
        requiredScenes.Add(SceneManager.GetSceneByName(levelSettings.defaultLevel));

        bool done = false;
        float progress = 0f;
        while (!done)
        {
            done = true;

            foreach (AsyncOperation op in ops)
            {
                if (!op.isDone)
                {
                    done = false;
                    break;
                }
            }

            foreach (AsyncOperation op in ops)
            {
                progress += op.progress;
            }

            progress /= ops.Count;

            bus.Publish(new LevelChangeProgress("Ładowanie gry", progress, progress));

            yield return null;
        }

        //sceneService.SetActiveScene(levelSettings.defaultLevel);
        currentLevel = levelSettings.defaultLevel;

        MovePlayer(levelSettings.defaultSpawnPoint);

        bus.Publish(new LevelChangeFinish());
    }

    public void ChangeLevel(string levelIndex, string spawnPointName)
    {
        mediatorObj.StartCoroutine(NewLevelCoroutine(levelIndex, spawnPointName));
    }

    private IEnumerator NewLevelCoroutine(string levelName, string spawnName)
    {
        bus.Publish(new LevelChangeBegin(levelName));

        if (!string.IsNullOrEmpty(currentLevel))
        {
            AsyncOperation unloadOp = sceneService.UnloadScene(currentLevel);
            while (!unloadOp.isDone)
            {
                bus.Publish(new LevelChangeProgress("Czyszczenie poprzedniego poziomu", unloadOp.progress, unloadOp.progress));
                yield return null;
            }
        }

        AsyncOperation loadOp = sceneService.LoadScene(levelName);
        Scene loadedScene = SceneManager.GetSceneByName(levelName);
        bool done = false;
        while (!done)
        {
            done = loadOp.isDone && loadedScene.IsValid() && loadedScene.isLoaded;
            bus.Publish(new LevelChangeProgress("Ładowanie kolejnego poziomu", loadOp.progress, loadOp.progress));
            yield return null;
        }

        //sceneService.SetActiveScene(levelName);
        currentLevel = levelName;

        MovePlayer(spawnName);

        bus.Publish(new LevelChangeFinish());
    }

    void MovePlayer(string destinationName)
    {
        if (player == null)
            player = Object.FindObjectOfType<PlayerReference>();

        GameObject spawnPoint = GameObject.Find(destinationName);

        if (player != null && spawnPoint != null)
            player.MovePlayer(spawnPoint.transform.position);
    }
}
