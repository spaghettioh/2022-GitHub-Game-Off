using UnityEngine;

public class ClumpController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _torque = 2f;
    [SerializeField] private float _maxSpeed = 7f;
    [Tooltip("Percentage of max speed to move in reverse")]
    [SerializeField] private float _reverseSpeedPercentage = .66f;

    [Header("Data and size")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private float _startingSize = 5f;

    private Camera _camera;
    private Rigidbody _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _clumpData.SetTransform(transform);
        _clumpData.SetSize(_startingSize);
        _camera = Camera.main;
    }

    private void Update()
    {
        var v = Input.GetAxisRaw("Vertical");
        if (v < 0)
        {
            v *= _reverseSpeedPercentage;
        }

        var moveDirection = _camera.transform.right * v;

        if (_body.velocity.magnitude < _maxSpeed)
        {
            _body.AddTorque(_torque * Time.deltaTime * moveDirection, ForceMode.Force);
        }
    }
}
