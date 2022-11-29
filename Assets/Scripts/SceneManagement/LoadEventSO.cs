using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
    menuName = "Game Off/Load Event Channel", fileName = "LoadEvent")]
public class LoadEventSO : ScriptableObject
{
    public UnityAction<string> OnSceneLoadRequested;
    public UnityAction<string> OnSceneLoadTransitionlessRequested;
    public UnityAction<string> OnRetrySceneLoadRequested;
    public UnityAction<string> OnWinSceneRequested;
    public UnityAction OnFollowingCutsceneRequested;

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
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{elevator} raised {name} but no one listens.");
        }
#endif
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
        {
            OnSceneLoadTransitionlessRequested.Invoke(sceneName);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{elevator} raised {name} (transitionless)" +
                $" but no one listens.");
        }
#endif
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
        {
            OnRetrySceneLoadRequested.Invoke(sceneName);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{elevator} raised {name} (retry)" +
                $" but no one listens.");
        }
#endif
    }

    public void RaiseWinScene(string followingCutscene) =>
       RaiseForRetry(followingCutscene, "Win event");
    public void RaiseWinScene(string followingCutscene,
        string elevator = "(Undefined)")
    {
        if (OnWinSceneRequested != null)
        {
            OnWinSceneRequested.Invoke(followingCutscene);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{elevator} raised {name} (win)" +
                $" but no one listens.");
        }
#endif
    }

    public void RaiseFollowingCutscene(string elevator = "(Undefined)")
    {
        if (OnFollowingCutsceneRequested != null)
        {
            OnFollowingCutsceneRequested.Invoke();
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{elevator} raised {name} (win)" +
                $" but no one listens.");
        }
#endif
    }
}