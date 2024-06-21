using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonMono<LevelManager>
{
    private List<EnemyController> enemys = new List<EnemyController>();

    public new void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += (scene, loadMode) =>
        {
            EventManager.EmitEvent(EventName.OnLevelLoaded);
        };
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        if(!enemys.Contains(enemy))
        {
            enemys.Add(enemy);
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        if (enemys.Contains(enemy))
        {
            enemys.Remove(enemy);
        }
    }

    public bool IsDefeatAllEnemys()
    {
        return enemys.Count == 0;
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevelAsync(int sceneIndex, UnityAction<float> callback)
    {
        StartCoroutine(RealyLoadLevelAsync(sceneIndex, callback));
    }
    public void LoadLevelAsync(int sceneIndex, UnityAction callback)
    {
        StartCoroutine(RealyLoadLevelAsync(sceneIndex, callback));
    }

    public void LoadLevelAsync(string sceneName, UnityAction<float> callback)
    {
        StartCoroutine(RealyLoadLevelAsync(sceneName, callback));
    }
    public void LoadLevelAsync(string sceneName, UnityAction callback)
    {
        StartCoroutine(RealyLoadLevelAsync(sceneName, callback));
    }

    private IEnumerator RealyLoadLevelAsync(int sceneIndex, UnityAction<float> callback)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            callback(asyncOperation.progress);
        }
        yield return null;
    }

    private IEnumerator RealyLoadLevelAsync(int sceneIndex, UnityAction callback)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        yield return asyncOperation;
        callback?.Invoke();
    }
    private IEnumerator RealyLoadLevelAsync(string sceneName, UnityAction<float> callback)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            callback(asyncOperation.progress);
        }
        yield return null;
    }

    private IEnumerator RealyLoadLevelAsync(string sceneName, UnityAction callback)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        yield return asyncOperation;
        callback?.Invoke();
    }
}
