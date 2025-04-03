using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class Loader
{
    public class DummyLoading: MonoBehaviour{}
    public enum SceneType
    {
        MainMenuScene,
        LoadingScene,
        GameScene
    }
    private static Action _onLoaderCallBack;
    private static AsyncOperation _loadingAsyncOperation;
    public static void LoadScene(SceneType type)
    {
        SceneManager.LoadScene(SceneType.LoadingScene.ToString());
        _onLoaderCallBack = () => {
            GameObject loadingObj = new GameObject("Loading object");
            loadingObj.AddComponent<DummyLoading>().StartCoroutine(LoadSceneAsync(type));
        };
    }
    public static IEnumerator LoadSceneAsync(SceneType type)
    {
        yield return null;
        _loadingAsyncOperation = SceneManager.LoadSceneAsync(type.ToString());
        if(!_loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if(_loadingAsyncOperation != null)
        {
            return _loadingAsyncOperation.progress;
        }
        return 1;
    }

    public static void OnLoaderCallBack()
    {
        if(_onLoaderCallBack == null) return;
        _onLoaderCallBack();
        _onLoaderCallBack = null;
    }
}
