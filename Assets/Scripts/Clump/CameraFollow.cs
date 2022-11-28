using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private Transform _t;

    private void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        _t.position = _clumpData.Transform.position;
    }

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
            SetPosition();
    }
}
