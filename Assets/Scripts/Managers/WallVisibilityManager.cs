using UnityEngine;

public class WallVisibilityManager : MonoBehaviour
{
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _wall;

    private void LateUpdate()
    {
        _wall.SetActive(true);
        if (_transform.position.z <= _clumpData.Transform.position.z &&
            _transform.position.x <= _clumpData.Transform.position.x)
        {
            _wall.SetActive(false);
        }
    }
}
