using System.Collections;
using UnityEngine;

public class ClumpController : MonoBehaviour
{
    private enum Mode
    {
        Gameplay,
        Cutscene
    }

    [SerializeField] private Mode _mode = Mode.Gameplay;
    [SerializeField] private float _crashStateDuration = 1f;

    [Header("Data and size")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private float _startingSize = 5f;

    [Header("Listening to...")]
    [SerializeField] private VoidEventSO _crashEvent;

    private Camera _camera;
    private Rigidbody _body;
    private SphereCollider _collider;
    private Vector3 cutsceneTorqueAxis = Vector3.right;
    private bool _isInCrashedState;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        _crashEvent.OnEventRaised += PauseMovement;
    }

    private void OnDisable()
    {
        _crashEvent.OnEventRaised -= PauseMovement;
    }

    private void Start()
    {
        _clumpData.SetUp(transform, _collider, _startingSize);
        _camera = Camera.main;
    }

    private void Update()
    {
        var torqueInput = _mode
            == Mode.Gameplay ? Input.GetAxisRaw("Vertical")
            : 1f;

        if (torqueInput < 0) torqueInput *= _clumpData.ReverseSpeedPercentage;

        Move(_mode == Mode.Gameplay ? _camera.transform.right * torqueInput
            : cutsceneTorqueAxis);
    }

    private void Move(Vector3 moveDirection)
    {
        if (_clumpData.Velocity < _clumpData.MaxSpeed && !_isInCrashedState)
        {
            _body.AddTorque(_clumpData.Torque * Time.deltaTime * moveDirection,
                ForceMode.Force);
        }

        _clumpData.SetVelocity(_body.velocity.magnitude);
    }

    private void PauseMovement() => StartCoroutine(PauseMovementRoutine());
    private IEnumerator PauseMovementRoutine()
    {
        if (_mode == Mode.Gameplay)
        {
            _isInCrashedState = true;
            yield return new WaitForSeconds(_crashStateDuration);
            _isInCrashedState = false;
        }
    }

    public void TutorialSteerLeft() =>
        StartCoroutine(TutorialSteerRoutine(Vector3.forward + Vector3.right));
    public void TutorialSteerRight() =>
        StartCoroutine(TutorialSteerRoutine(Vector3.back + Vector3.right));
    private IEnumerator TutorialSteerRoutine(Vector3 heading)
    {
        yield return new WaitForSeconds(.1f);
        cutsceneTorqueAxis = heading;
    }
}
