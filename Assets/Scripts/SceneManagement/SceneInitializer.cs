using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private VoidEventSO _skipCurtains;

    [Header("PersistentManagers")]
    [SerializeField] private string _persistentManagersSceneName = "PersistentManagers";

    private void Start()
    {
        // Load the managers scene
        if (!SceneManager.GetSceneByName(_persistentManagersSceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(_persistentManagersSceneName,
                LoadSceneMode.Additive).completed += PersistentManagersLoaded;
        }
    }

    private void PersistentManagersLoaded(AsyncOperation unused = default)
    {
        _skipCurtains.Raise(name);
    }
}