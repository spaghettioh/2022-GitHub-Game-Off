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
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private Rigidbody _body;

    [Header("Data and size")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private float _startingSize;

    [Header("Listening to...")]
    [SerializeField] private VoidEventSO _crashEvent;
    [SerializeField] private PauseGameplayEventSO _pauseGameplay;

    private Camera _camera;
    private Vector3 _cutsceneTorqueAxis = Vector3.right;
    private bool _canMove = true;

    private void Awake()
    {
        //_body = GetComponent<Rigidbody>();
        //_collider = GetComponent<SphereCollider>();
        _clumpData.SetUp(transform, _collider, _startingSize);
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
        _camera = Camera.main;
    }

    private void Update()
    {
        var torqueInput = _mode
            == Mode.Gameplay ? Input.GetAxisRaw("Vertical") : 1f;

        if (torqueInput < 0) torqueInput *= _clumpData.ReverseSpeedPercentage;

        Move(_mode == Mode.Gameplay ? _camera.transform.right * torqueInput
            : _cutsceneTorqueAxis);
    }

    private void Move(Vector3 moveDirection)
    {
        if (_clumpData.Velocity < _clumpData.MaxSpeed && _canMove)
        {
            _body.AddTorque(_clumpData.Torque * Time.deltaTime * moveDirection,
                ForceMode.Force);
        }

        _clumpData.SetVelocity(_body.velocity.magnitude);
    }

    private void PauseMovement(bool pause, bool shouldStopMovement)
    {
        _canMove = !pause;

        if (shouldStopMovement)
        {
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
        if (_mode == Mode.Gameplay)
        {
            _canMove = false;
            yield return new WaitForSeconds(_crashStateDuration);
            _canMove = true;
        }
    }

    public void TutorialSteerLeft() =>
        StartCoroutine(TutorialSteerRoutine(Vector3.forward + Vector3.right));
    public void TutorialSteerRight() =>
        StartCoroutine(TutorialSteerRoutine(Vector3.back + Vector3.right));
    private IEnumerator TutorialSteerRoutine(Vector3 heading)
    {
        yield return new WaitForSeconds(.1f);
        _cutsceneTorqueAxis = heading;
    }
}
