using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[CreateAssetMenu (fileName = "ScaleEvent", menuName = "Game Off/Scale Event")]
public class ScaleEventSO : ScriptableObject
{
    public UnityAction OnScaleChanged;

    public void Raise()
    {
        if (OnScaleChanged != null)
        {
            OnScaleChanged.Invoke();
        }
        else
        {
            Debug.LogWarning("Scale Event raised but no one listens...");
        }
    }
}
