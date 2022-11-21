using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatEvent_NAME"
    , menuName = "Game Off/Float Event")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void Raise(float value, string elevator = "(Unknown)")
    {
        if (OnEventRaised != null) OnEventRaised.Invoke(value);
        else Debug.LogWarning($"{elevator} raised {name} but no one listens.");
    }
}
