using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Transform_NAME"
    , menuName = "Game Off/Transform Anchor")]
public class TransformAnchorSO : ScriptableObject
{
    private Transform _transform;

    public Transform Transform
    {
        get { return _transform; }
    }

    public void Set(Transform value)
    {
        _transform = value;
    }
}
