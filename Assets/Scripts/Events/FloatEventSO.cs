using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
    fileName = "FloatEvent_NAME", menuName = "Game Off/Float Event")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void Raise(float value, string elevator = "(Unknown)")
    {
        Debug.Log($"{elevator} raised {name}");
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{elevator} raised {name} but no one listens.");
        }
#endif
    }
}
