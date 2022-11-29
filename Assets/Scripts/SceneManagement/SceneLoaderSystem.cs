using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSystem : MonoBehaviour
{
    [SerializeField] private LoadEventSO _loadEventChannel;
    [SerializeField] private VoidEventSO _closeCurtains;
    [SerializeField] private VoidEventSO _curtainsClosed;
    [SerializeField] private VoidEventSO _sceneLoaded;
    [SerializeField] private string _winSceneName;
    [SerializeField] private string _fallbackScene = "UI_Title";

    [Header("DEBUG ==========")]
    [SerializeField] private string _currentActiveScene;
    [SerializeField] private string _nextScene;
    [SerializeField] private string _followingCutscene;

    private void OnEnable()
    {
        _loadEventChannel.OnSceneLoadRequested += TransitionToNextScene;
        _loadEventChannel.OnWinSceneRequested += TransitionToWinScene;
        _loadEventChannel.OnFollowingCutsceneRequested
            += TransitionToFollowingCutscene;
        _loadEventChannel.OnSceneLoadTransitionlessRequested
            += LoadSceneImmediately;
    }

    private void OnDisable()
    {
        _loadEventChannel.OnSceneLoadRequested -= TransitionToNextScene;
        _loadEventChannel.OnWinSceneRequested -= TransitionToWinScene;
        _loadEventChannel.OnFollowingCutsceneRequested
            -= TransitionToFollowingCutscene;
        _loadEventChannel.OnSceneLoadTransitionlessRequested
            -= LoadSceneImmediately;
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

    private void TransitionToNextScene(string newScene)
    {
        _currentActiveScene = SceneManager.GetActiveScene().name;
        _nextScene = newScene;
        _closeCurtains.Raise(name);
        // Subscribe to the screen transition
        _curtainsClosed.OnEventRaised += LoadScene;
    }

    private void TransitionToWinScene(string followingCutscene)
    {
        _followingCutscene = followingCutscene;
        TransitionToNextScene(_winSceneName);
    }

    private void TransitionToFollowingCutscene()
    {
        if (_followingCutscene == "")
        {
            _followingCutscene = _fallbackScene;
        }
        TransitionToNextScene(_followingCutscene);
        _followingCutscene = "";
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