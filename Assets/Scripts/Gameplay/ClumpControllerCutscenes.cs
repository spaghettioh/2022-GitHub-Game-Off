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

    private Vector3 torqueDirection = Vector3.right;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _clumpData.SetTransform(transform);
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
