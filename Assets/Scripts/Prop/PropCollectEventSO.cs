using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
    fileName = "PropCollectEvent", menuName = "Game Off/Prop Collect Event")]
public class PropCollectEventSO : ScriptableObject
{
    public UnityAction<Prop> OnEventRaised;

    public void Raise(Prop prop, string elevator = "(Unknown)")
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(prop);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{elevator} raised {name} but no one listens.");
        }
#endif
    }
}
