using System.Collections;
using UnityEngine;

public class ClumpControllerNew : MonoBehaviour
{
    [Header("Basic setup")]
    [SerializeField] private float _crashStateDuration = 1f;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private Rigidbody _body;
    [SerializeField] private Transform _telemetryObject;
    [SerializeField] private TransformAnchorSO _cameraTransform;

    [Header("Movement")]
    [SerializeField] private bool _showTelemetry;
    [SerializeField] private ForceMode _forceMode;

    [Header("Data and size")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private float _startingSize;
    [SerializeField] private float _startingTorque;

    [Header("Listening to...")]
    [SerializeField] private VoidEventSO _crashEvent;
    [SerializeField] private PauseGameplayEventSO _pauseGameplay;
    [SerializeField] private InputHandlerSO _inputSO;

    [Header("DEBUG ==========")]
    [SerializeField] private bool _canMove = true;
    [SerializeField] private float _velocity;
    [SerializeField] private float _angularVelocity;

    private Vector3 _input;

    private void Awake()
    {
        //_clumpData.ConfigureData(transform, _collider, _startingSize, _startingTorque);
        _telemetryObject.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _crashEvent.OnEventRaised += Crashed;
        _pauseGameplay.OnEventRaised += PauseMovement;
    }

    private void OnDisable()
    {
        _crashEvent.OnEventRaised -= Crashed;
        _pauseGameplay.OnEventRaised -= PauseMovement;
    }

    private void Start()
    {
        _telemetryObject.SetParent(_cameraTransform.Transform);
        _telemetryObject.localPosition = new Vector3(-3, -1.5f, 3);
    }

    private void Update()
    {
        var v = Input.GetAxisRaw("Vertical");
        var h = Input.GetAxisRaw("Horizontal");
        _input = new Vector3(
            _inputSO.DirectionalInput.x, 0, _inputSO.DirectionalInput.y);
        //_input = new Vector3(h, 0, v);

        SetTorqueDirection();

        _telemetryObject.gameObject.SetActive(_showTelemetry);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void SetTorqueDirection()
    {
        if (_input != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var skewedInput = matrix.MultiplyPoint3x4(_input);

            var relative = (_telemetryObject.position + skewedInput)
                - _telemetryObject.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            _telemetryObject.rotation = Quaternion.RotateTowards(
                _telemetryObject.rotation, rot, 360);
        }
    }

    private void Move()
    {
        if (_input != Vector3.zero && _canMove)
        {
            _body.AddForce(_startingTorque * Time.deltaTime * _telemetryObject.forward,
            //_body.AddTorque(_clumpData.Torque * Time.deltaTime * _telemetryObject.right,
                _forceMode);
        }

        _clumpData.SetVelocity(_body.velocity.magnitude);
        _velocity = _body.velocity.magnitude;
        _angularVelocity = _body.angularVelocity.magnitude;
    }

    private void PauseMovement(bool pause, bool shouldStopMovement)
    {
        _canMove = !pause;

        if (shouldStopMovement)
        {
            // TODO this doesn't seem to work properly.
            // When the game is paused to scale up, you can still move kinda
            _body.Sleep();
            _body.useGravity = false;
            _collider.enabled = false;
        }
        else
        {
            _collider.enabled = true;
            _body.useGravity = true;
        }
    }

    private void Crashed() => StartCoroutine(CrashedRoutine());
    private IEnumerator CrashedRoutine()
    {
        _canMove = false;
        yield return new WaitForSeconds(_crashStateDuration);
        _canMove = true;
    }
}
