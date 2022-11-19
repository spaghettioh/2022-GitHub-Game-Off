using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private LoadEventSO _loadEventChannel;
    [SerializeField] private string _titleSceneName;
    [Header("PersistentManagers")]
    [SerializeField] private string _persistentManagersSceneName = "PersistentManagers";

    private void Start()
    {
        if (!SceneManager.GetSceneByName(_persistentManagersSceneName).isLoaded)
        {
            // Load the managers scene and scubscribe to its complete event
            // so that the EventSystem works properly
            SceneManager.LoadSceneAsync(_persistentManagersSceneName,
                LoadSceneMode.Additive).completed += PersistentManagersLoaded;
        }
    }

    private void PersistentManagersLoaded(AsyncOperation unused)
    {
        _loadEventChannel.Raise(_titleSceneName, name);
    }
}