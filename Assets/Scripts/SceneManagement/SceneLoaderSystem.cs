using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSystem : MonoBehaviour
{
    [SerializeField] private LoadEventSO _loadEventChannel;
    [SerializeField] private VoidEventSO _closeCurtains;
    [SerializeField] private VoidEventSO _curtainsClosed;
    [SerializeField] private VoidEventSO _sceneLoaded;

    private string _currentActiveScene;
    private string _nextScene;

    private void OnEnable()
    {
        _loadEventChannel.OnSceneLoadRequested += TransitionToNewScene;
        _loadEventChannel.OnSceneLoadTransitionlessRequested
            += LoadSceneImmediately;
    }

    private void OnDisable()
    {
        _loadEventChannel.OnSceneLoadRequested -= TransitionToNewScene;
        _loadEventChannel.OnSceneLoadTransitionlessRequested
            -= LoadSceneImmediately;
    }

    private void Start()
    {
        _sceneLoaded.Raise($"{name} via Start()");
    }

    private void LoadSceneImmediately(string newScene) =>
        StartCoroutine(LoadSceneImmediatelyRoutine(newScene));
    private IEnumerator LoadSceneImmediatelyRoutine(string newScene)
    {
        _currentActiveScene = SceneManager.GetActiveScene().name;
        _nextScene = newScene;

        SceneManager.UnloadSceneAsync(_currentActiveScene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(
            _nextScene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_nextScene));
        _currentActiveScene = _nextScene;

        _sceneLoaded.Raise(name);
    }

    private void TransitionToNewScene(string newScene)
    {
        _currentActiveScene = SceneManager.GetActiveScene().name;
        _nextScene = newScene;
        _closeCurtains.Raise(name);
        // Subscribe to the screen transition
        _curtainsClosed.OnEventRaised += LoadScene;
    }

    private void LoadScene() => StartCoroutine(LoadSceneRoutine());
    private IEnumerator LoadSceneRoutine()
    {
        // Unsubscribe from the screen transition
        _curtainsClosed.OnEventRaised -= LoadScene;

        SceneManager.UnloadSceneAsync(_currentActiveScene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(
            _nextScene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_nextScene));
        _currentActiveScene = _nextScene;

        _sceneLoaded.Raise(name);
    }
}