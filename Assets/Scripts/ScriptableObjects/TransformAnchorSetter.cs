using UnityEngine;

public class TransformAnchorSetter : MonoBehaviour
{
    [SerializeField] private TransformAnchorSO _transform;
    private void Start()
    {
        _transform.Set(transform);
    }
}
