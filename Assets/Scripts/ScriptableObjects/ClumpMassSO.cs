using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ClumpMass", menuName = "Game Off/ClumpMass")]
public class ClumpMassSO : ScriptableObject
{
    [field: SerializeField]
    public float Mass { get; private set; }
    public UnityAction<float> OnMassChanged;

    public void Set(float value)
    {
        Mass = value;
        AlertChange();
    }

    public void Increase(float value)
    {
        Mass += value;
        AlertChange();
    }

    public void Decrease(float value)
    {
        Mass -= value;
        AlertChange();
    }

    private void AlertChange()
    {
        OnMassChanged.Invoke(Mass);
    }
}
