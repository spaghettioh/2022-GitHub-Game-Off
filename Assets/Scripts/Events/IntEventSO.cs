using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Int_NAME"
    , menuName = "Game Off/Int Event")]
public class IntEventSO : ScriptableObject
{
    public UnityAction<int> OnEventRaised;

    public void Raise(int value, string elevator = "(Unknown)")
    {
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
