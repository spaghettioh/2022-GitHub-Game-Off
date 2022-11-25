using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private VoidEventSO _openCurtains;
    [SerializeField] private string _persistentManagersSceneName = "PersistentManagers";

    [Header("DEBUG ==========")]
    [SerializeField] private bool _openCurtainsRequested;

    private void OnEnable()
    {
        _openCurtains.OnEventRaised += CheckForCurtains;
    }

    private void Start()
    {
        // Load the managers scene
        if (!SceneManager.GetSceneByName(_persistentManagersSceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(_persistentManagersSceneName,
                LoadSceneMode.Additive).completed += PersistentManagersLoaded;
        }
        else
        {
            PersistentManagersLoaded();
        }
    }

    private void PersistentManagersLoaded(AsyncOperation unused = default)
    {
        if (!_openCurtainsRequested) _openCurtains.Raise(name);
    }

    private void CheckForCurtains()
    {
        _openCurtainsRequested = true;
    }
}