using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSystem : MonoBehaviour
{
    [SerializeField] private LoadEventSO _loadEventChannel;
    [SerializeField] private VoidEventSO _startWaxOn;
    [SerializeField] private VoidEventSO _waxOnFinished;
    [SerializeField] private VoidEventSO _startWaxOff;

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
        _startWaxOn.Raise();
        // Subscribe to the screen wipe finish
        _waxOnFinished.OnEventRaised += TriggerNewScene;
    }

    private void TriggerNewScene()
    {
        // Unsubscribe from the screen wipe finish
        _waxOnFinished.OnEventRaised -= TriggerNewScene;
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

        _startWaxOff.Raise();
    }
}