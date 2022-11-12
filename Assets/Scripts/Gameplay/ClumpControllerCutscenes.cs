using System.Collections;
using UnityEngine;

public class ClumpControllerCutscenes : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _torque = 2f;
    [SerializeField] private float _maxSpeed = 7f;

    [Header("Data and size")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private float _startingSize = 2f;

    private Rigidbody _body;
    private SphereCollider _collider;

    private Vector3 torqueDirection = Vector3.right;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        _clumpData.SetUp(transform, _collider);
        _clumpData.SetSize(_startingSize);
    }

    private void Update()
    {
        if (_body.velocity.magnitude < _maxSpeed)
        {
            _body.AddTorque(_torque * Time.deltaTime * torqueDirection, ForceMode.Force);
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
        torqueDirection = heading;
    }
}
