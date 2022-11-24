using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Void_NAME", menuName = "Game Off/Void Event")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void Raise(string elevator = "(Undefined)")
    {
        Debug.Log($"{elevator} raised {name}");
        if (OnEventRaised != null) OnEventRaised.Invoke();
        else Debug.LogWarning(
            $"{elevator} raised {name} but no one listens.");
    }
}
