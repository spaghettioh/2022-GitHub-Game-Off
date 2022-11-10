using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSystem : MonoBehaviour
{
    [SerializeField] private LoadEventSO _loadEventChannel;
    [SerializeField] private VoidEventSO _closeCurtainsEvent;
    [SerializeField] private VoidEventSO _curtainsClosed;
    [SerializeField] private VoidEventSO _loadNextScene;

    private string _currentActiveScene;
    private string _nextScene;

    private void OnEnable()
    {
        _loadEventChannel.OnSceneLoadRequested += RequestNewScene;
    }

    private void OnDisable()
    {
        _loadEventChannel.OnSceneLoadRequested -= RequestNewScene;
    }

    private void RequestNewScene(string newScene)
    {
        _currentActiveScene = SceneManager.GetActiveScene().name;
        _nextScene = newScene;
        _closeCurtainsEvent.Raise();
        // Subscribe to the screen wipe finish
        _curtainsClosed.OnEventRaised += TriggerNewScene;
    }

    private void TriggerNewScene()
    {
        // Unsubscribe from the screen wipe finish
        _curtainsClosed.OnEventRaised -= TriggerNewScene;
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        SceneManager.UnloadSceneAsync(_currentActiveScene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(
            _nextScene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_nextScene));
        _currentActiveScene = _nextScene;

        _loadNextScene.Raise();
    }
}