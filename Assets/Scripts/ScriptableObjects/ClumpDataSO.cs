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

    public float SizeInMeters
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
        SizeInMeters = Mathf.Round(value * 100f) / 100f;
        AlertSizeChange();
    }

    public void IncreaseSize(float value)
    {
        SizeInMeters += Mathf.Round(value * 1000f) / 1000f;
        AlertSizeChange();
    }

    public void DecreaseSize(float value)
    {
        SizeInMeters -= Mathf.Round(value * 1000f) / 1000f;
        AlertSizeChange();
    }

    private void AlertSizeChange()
    {
        if (OnSizeChanged != null)
        {
            OnSizeChanged.Invoke(SizeInMeters);
        }
        else
        {
            Debug.LogWarning($"Nobody heard size change event from {name}");
        }
    }
}
