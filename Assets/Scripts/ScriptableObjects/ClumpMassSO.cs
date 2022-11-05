using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClumpMass", menuName = "Game Off/ClumpMass")]
public class ClumpMassSO : ScriptableObject
{
    private float _mass;

    public float Mass
    {
        get { return _mass; }
    }

    public void Set(float value)
    {
        _mass = value;
    }

    public void Increase(float value)
    {
        _mass += value;
    }

    public void Decrease(float value)
    {
        _mass -= value;
    }
}
