using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="PauseEvent", menuName = "Game Off/Pause event")]
public class PauseGameplayEventSO : ScriptableObject
{
    public UnityAction<bool, bool> OnEventRaised;

    public void Raise(bool pause,
        bool shouldStopMovement = false, string elevator = "(Unknown)")
    {
        if (OnEventRaised != null) OnEventRaised
                .Invoke(pause, shouldStopMovement);
        else Debug.LogWarning($"{elevator} raised {name} (pause: {pause}," +
            $" shouldStopMovement: {shouldStopMovement}) " +
            $"but no one listens.");
    }

    public void Raise(bool pause, string elevator = "(Unknown)") =>
        Raise(pause, false, elevator);
}
