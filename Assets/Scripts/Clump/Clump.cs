using System.Collections;
using UnityEngine;

public class Clump : ClumpController
{
    [Header("Clump setup")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private VoidEventSO _crashEvent;
    [SerializeField] private float _crashStateDuration = 1f;

    [Header("Listening to...")]
    [SerializeField] private InputHandlerSO _inputHandler;
    [SerializeField] private PauseGameplayEventSO _pauseGameplay;

    [Header("DEBUG ==========")]
    [SerializeField] private bool _canMove;

    private void Awake()
    {
        TryGetComponent(out Collider);
        TryGetComponent(out Body);
        _clumpData.ConfigureData(transform, Collider);
        ConfigureController(_clumpData.MinColliderRadius,
            _clumpData.MinMoveForce, _clumpData.MaxSpeed);
    }

    private void OnEnable()
    {
        _crashEvent.OnEventRaised += Crashed;
        _pauseGameplay.OnEventRaised += PauseMovement;
        _inputHandler.OnDirectionalInput += SendDirectionalInput;
        _clumpData.OnStatsChanged += SetMoveConfig;
    }

    private void OnDisable()
    {
        _crashEvent.OnEventRaised -= Crashed;
        _pauseGameplay.OnEventRaised -= PauseMovement;
        _inputHandler.OnDirectionalInput -= SendDirectionalInput;
        _clumpData.OnStatsChanged -= SetMoveConfig;
    }

    public override void Update()
    {
        base.Update();
        _clumpData.SetVelocity(Body.velocity.magnitude);
    }

    private void SendDirectionalInput(Vector2 input)
    {
        if (_canMove)
        {
            SetDirectionalInput(input.normalized);
        }
        else
        {
            SetDirectionalInput(Vector2.zero);
        }
    }

    private void PauseMovement(bool pause, bool shouldStopMovement)
    {
        _canMove = !pause;
        if (shouldStopMovement)
        {
            Body.Sleep();
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
