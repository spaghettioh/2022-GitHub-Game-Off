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
    private SphereCollider _collider;
    private Vector3 cutsceneTorqueAxis = Vector3.right;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        _clumpData.SetUp(transform, _collider);
        _clumpData.SetSize(_startingSize);
        _camera = Camera.main;
    }

    private void Update()
    {
        var v = _mode
            == Mode.Gameplay ? Input.GetAxisRaw("Vertical")
            : 1f;

        if (v < 0) v *= _reverseSpeedPercentage;

        var moveDirection = _mode
            == Mode.Gameplay ? _camera.transform.right * v
            : cutsceneTorqueAxis;

        if (_body.velocity.magnitude < _maxSpeed)
        {
            _body.AddTorque(_torque * Time.deltaTime * moveDirection, ForceMode.Force);
        }
    }

    public void TutorialSteerLeft()
    {
        StartCoroutine(TutorialSteerRoutine(Vector3.forward + Vector3.right));
    }

    public void TutorialSteerRight()
    {
        StartCoroutine(TutorialSteerRoutine(Vector3.back + Vector3.right));
    }

    private IEnumerator TutorialSteerRoutine(Vector3 heading)
    {
        yield return new WaitForSeconds(.2f);
        cutsceneTorqueAxis = heading;
    }
}
