using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatEvent_NAME", menuName = "Game Off/Float Event")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void Raise(float value)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value);
        }
        else
        {
            Debug.LogWarning($"Nothing heard Float Event: {name}");
        }
    }
}
