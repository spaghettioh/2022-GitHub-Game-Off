using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    [Header("Cold start")]
    [SerializeField]
    private string _persistentManagersSceneName = "PersistentManagers";
    [SerializeField] private VoidEventSO _skipCurtains;
    [SerializeField] private ScoreSO _score;

    [Header("Scene transitions")]
    [SerializeField] private VoidEventSO _sceneLoaded;
    [SerializeField] private UnityEvent _onSceneLoaded;
    [Space]
    [SerializeField] private VoidEventSO _openCurtains;
    [SerializeField] private VoidEventSO _curtainsOpened;
    [SerializeField] private UnityEvent _onCurtainsOpened;

    private void OnEnable()
    {
        _sceneLoaded.OnEventRaised += OnSceneLoaded;
        _curtainsOpened.OnEventRaised += OnCurtainsOpened;
    }

    private void OnDisable()
    {
        _sceneLoaded.OnEventRaised -= OnSceneLoaded;
        _curtainsOpened.OnEventRaised -= OnCurtainsOpened;
    }

    private void Start()
    {
        // Load the managers scene
        if (!SceneManager.GetSceneByName(_persistentManagersSceneName).isLoaded)
        {
            _score.Reset();
            SceneManager.LoadSceneAsync(_persistentManagersSceneName,
                LoadSceneMode.Additive).completed += OnPersistentManagersLoaded;
        }
    }

    private void OnPersistentManagersLoaded(AsyncOperation ignore)
    {
        var message = $"{name}, on cold start after loading PeristentManagers,";
        // Unsubscribe on cold starts
        _sceneLoaded.OnEventRaised -= OnSceneLoaded;
        _sceneLoaded.Raise(message);
        _skipCurtains.Raise(message);
    }

    private void OnSceneLoaded()
    {
        _onSceneLoaded.Invoke();
        _openCurtains.Raise($"{name}, after being loaded,");
    }

    private void OnCurtainsOpened()
    {
        _onCurtainsOpened.Invoke();
    }
}