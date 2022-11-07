using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;

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
        transform.rotation = Quaternion.Euler(cam);
    }

    private void SetMainCamera()
    {
        _mainCamera = Camera.main;
    }
}
