using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _follow;
    [SerializeField] private Vector3 _followOffset;
    [SerializeField] private Camera _camera;

    void LateUpdate()
    {
        SetPosition();
        transform.Rotate(Vector3.up, Input.GetAxisRaw("Horizontal"));
    }

    private void SetPosition()
    {
        transform.position = _follow.position;
        _camera.transform.localPosition = _followOffset;
        _camera.orthographicSize = _followOffset.y;
    }

    private void OnValidate()
    {
        SetPosition();
    }
}
