using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private Vector3 _followOffset;
    [SerializeField] private float _offsetSmoothing;
    [SerializeField] private Camera _camera;

    [SerializeField] private int _sensitivity = 80;
    [SerializeField] private PauseGameplayEventSO _pauseEvent;

    private bool _canRotate = true;
    private float _currentOffsetZ;

    private void OnEnable()
    {
        _pauseEvent.OnEventRaised += PauseRotation;
    }

    private void OnDisable()
    {
        _pauseEvent.OnEventRaised -= PauseRotation;
    }

    void LateUpdate()
    {
        if (_canRotate)
        {
            transform.Rotate(Vector3.up,
                Input.GetAxisRaw("Horizontal") * Time.deltaTime * _sensitivity);

            var temp = _followOffset.z;

            //if (Input.GetAxisRaw("Vertical") < 0)
            //{
            //    temp = _followOffset.z * -1f;
            //}
            //Mathf.Lerp(_followOffset.z, temp, Time.deltaTime * _offsetSmoothing);
            //else
            //{
            //    Mathf.Lerp(_currentOffsetZ, _followOffset)
            //}
            SetPosition();

        }
    }

    private void PauseRotation(bool pause, bool shouldStopMovement)
    {
        _canRotate = !pause;
    }

    private void SetPosition()
    {
        transform.position = _clumpData.Transform.position;
        _camera.transform.localPosition = _followOffset;
        _camera.orthographicSize = _followOffset.y;
    }

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy) SetPosition();
    }
}
