using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraFollow : MonoBehaviour
{
    [SerializeField] private TransformAnchorSO _clumpTransform;
    [SerializeField] private Vector3 _offset;
    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _clumpTransform.Transform.position + _offset;
        transform.LookAt(_clumpTransform.Transform.position);
    }

    private void OnValidate()
    {
        transform.position = _clumpTransform.Transform.position + _offset;
        transform.LookAt(_clumpTransform.Transform.position);

    }
}
