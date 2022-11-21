using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAnchorSetter : MonoBehaviour
{
    [SerializeField] private TransformAnchorSO _transform;

    void Start()
    {
        _transform.Set(transform);
    }
}
