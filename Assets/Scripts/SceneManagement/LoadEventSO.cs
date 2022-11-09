using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game Off/Scriptable Objects/Load Event Channel",
    fileName = "LoadEvent")]
public class LoadEventSO : ScriptableObject
{
    public UnityAction<string> OnSceneLoadRequested;

    public void Raise(string sceneName)
    {
        OnSceneLoadRequested.Invoke(sceneName);
    }
}