using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Int_NAME"
    , menuName = "Game Off/Int Event")]
public class IntEventSO : ScriptableObject
{
    public UnityAction<int> OnEventRaised;

    public void Raise(int value, string elevator = "(Unknown)")
    {
        Debug.Log($"{elevator} raised {name}");
        if (OnEventRaised != null) OnEventRaised.Invoke(value);
        else Debug.LogWarning($"{elevator} raised {name} but no one listens.");
    }
}
