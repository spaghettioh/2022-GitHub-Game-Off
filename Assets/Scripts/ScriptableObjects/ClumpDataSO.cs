using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ClumpData", menuName = "Game Off/ClumpData")]
public class ClumpDataSO : ScriptableObject
{
    public UnityAction<float> OnSizeChanged;

    public Transform Transform
    {
        get;
        private set;
    }

    public float Mass
    {
        get;
        private set;
    }

    public void SetTransform(Transform t)
    {
        Transform = t;
    }

    public void SetSize(float value)
    {
        Mass = value;
        AlertSizeChange();
    }

    public void IncreaseSize(float value)
    {
        Mass += value;
        AlertSizeChange();
    }

    public void DecreaseSize(float value)
    {
        Mass -= value;
        AlertSizeChange();
    }

    private void AlertSizeChange()
    {
        if (OnSizeChanged != null)
        {
            OnSizeChanged.Invoke(Mass);
        }
        else
        {
            Debug.LogWarning($"Nobody heard size change event from {name}");
        }
    }
}
