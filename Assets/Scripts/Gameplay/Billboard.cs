using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private bool _faceCameraOnly;

    private void Start()
    {
        SetMainCamera();
    }

    private void Update()
    {
        FaceCamera();
    }

    private void OnValidate()
    {
        SetMainCamera();
        FaceCamera();
    }

    private void FaceCamera()
    {
        var cam = _mainCamera.transform.rotation.eulerAngles;
        if (_faceCameraOnly)
        {
            transform.rotation = Quaternion.Euler(cam.x, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(cam);

        }
    }

    private void SetMainCamera()
    {
        _mainCamera = Camera.main;
    }
}
