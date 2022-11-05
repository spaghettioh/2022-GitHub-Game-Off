using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Void_NAME", menuName = "Game Off/Void Event")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void Raise()
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke();
        }
        else
        {
            Debug.LogWarning($"Nothing heard Void Event: {name}");
        }
    }
}
