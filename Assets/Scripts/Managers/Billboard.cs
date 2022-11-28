using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private bool _canLean;
    [SerializeField] private bool _isProp;
    private Camera _mainCamera;

    private void Start()
    {
        SetMainCamera();
        FaceCamera();
    }

    private void Update()
    {
        FaceCamera();
    }

    private void FaceCamera()
    {
        var cam = _mainCamera.transform.rotation.eulerAngles;
        if (!_canLean)
        {
            cam.x = 0f;
            cam.z = 0f;
        }

        if (_isProp)
        {
            cam.x += -90;
        }

        transform.rotation = Quaternion.Euler(cam);
    }

    private void SetMainCamera()
    {
        _mainCamera = Camera.main;
    }

    public void OrientNow()
    {
        if (gameObject.activeInHierarchy)
        {
            SetMainCamera();
            FaceCamera();
        }
    }
}
