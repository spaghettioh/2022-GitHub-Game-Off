using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ClumpData", menuName = "Game Off/ClumpData")]
public class ClumpDataSO : ScriptableObject
{
    public UnityAction<float> OnSizeChanged;

    public Transform Transform { get; private set; }
    public float Size { get; private set; }

    public void SetTransform(Transform t)
    {
        Transform = t;
    }

    public void SetSize(float value)
    {
        Size = value;
        Announce();
    }

    public void IncreaseSize(float value)
    {
        Size += value;
        Announce();
    }

    public void DecreaseSize(float value)
    {
        Size -= value;
        Announce();
    }

    private void Announce()
    {
        if (OnSizeChanged != null)
        {
            OnSizeChanged.Invoke(Size);
        }
        else
        {
            Debug.LogWarning($"Nobody heard size change event from {name}");
        }
    }
}
