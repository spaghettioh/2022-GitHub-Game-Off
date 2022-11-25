using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private VoidEventSO _openCurtains;

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
        _openCurtains.Raise(name);
    }
}