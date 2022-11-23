using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Load Event Channel",
    fileName = "LoadEvent")]
public class LoadEventSO : ScriptableObject
{
    public UnityAction<string> OnSceneLoadRequested;
    public UnityAction<string> OnSceneLoadTransitionlessRequested;
    public UnityAction<string> OnRetrySceneLoadRequested;

    /// <summary>
    /// Usually used by UI elements or event listeners
    /// because events only take one argument
    /// </summary>
    /// <param name="sceneName"></param>
    public void Raise(string sceneName) =>
        Raise(sceneName, "Another event (likely UI or listener)");
    public void Raise(string sceneName, string elevator = "(Undefined)")
    {
        if (OnSceneLoadRequested != null)
        {
            OnSceneLoadRequested.Invoke(sceneName);
        }
        else
        {
            Debug.LogWarning($"{elevator} raised {name} but no one listens.");
        }
    }

    /// <summary>
    /// Usually used by UI elements or event listeners
    /// because events only take one argument
    /// </summary>
    /// <param name="sceneName"></param>
    public void RaiseTransitionless(string sceneName) =>
        RaiseTransitionless(sceneName, "Another event (likely UI or listener)");
    public void RaiseTransitionless(
        string sceneName, string elevator = "(Undefined)")
    {
        if (OnSceneLoadTransitionlessRequested != null)
            OnSceneLoadTransitionlessRequested.Invoke(sceneName);
        else
            Debug.LogWarning($"{elevator} raised {name} (transitionless)" +
                $" but no one listens.");
    }

    /// <summary>
    /// Usually used by UI elements or event listeners
    /// because events only take one argument
    /// </summary>
    /// <param name="sceneName"></param>
    public void RaiseForRetry(string sceneName) =>
        RaiseForRetry(sceneName, "Another event (likely UI or listener)");
    public void RaiseForRetry(
        string sceneName, string elevator = "(Undefined)")
    {
        if (OnRetrySceneLoadRequested != null)
            OnRetrySceneLoadRequested.Invoke(sceneName);
        else
            Debug.LogWarning($"{elevator} raised {name} (retry)" +
                $" but no one listens.");
    }
}