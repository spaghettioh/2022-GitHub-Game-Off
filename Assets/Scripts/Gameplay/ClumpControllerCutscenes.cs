using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClumpControllerCutscenes : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _torque = 2f;
    [SerializeField] private float _maxSpeed = 7f;
    [Tooltip("Percentage of max speed to move in reverse")]
    [SerializeField] private float _reverseSpeedPercentage = .66f;

    [Header("Data and size")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private float _startingSizeInMeters = 5f;
    [Tooltip("Collider radius change on size whole numbers")]
    [SerializeField] private float _colliderChange = .1f;

    [Header("Collection")]
    [SerializeField] private CollectEventSO _collectEvent;
    [SerializeField] private VoidEventSO _knockEvent;
    [SerializeField] private List<Collectible> _collectibles = new List<Collectible>();
    [Tooltip("Collected mass percentage added to clump")]
    [SerializeField] private float _collectionModifier = .1f;

    private Rigidbody _body;
    private SphereCollider _collider;
    [SerializeField] private Vector3 torqueDirection = Vector3.right;

    private void OnEnable()
    {
        _collectEvent.OnCollected = CollectSomething;
        //_knockEvent.OnEventRaised = LoseSomething;
    }

    private void OnDisable()
    {
        _collectEvent.OnCollected -= CollectSomething;
        //_knockEvent.OnEventRaised -= LoseSomething;
    }

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        _clumpData.SetTransform(transform);
        _clumpData.SetSize(_startingSizeInMeters);
        TryGetComponent(out _collider);
    }

    private void CollectSomething(Collectible collectible)
    {
        var currentMass = _clumpData.SizeInMeters;

        _clumpData.IncreaseSize(collectible.CollectedSize);
        _collectibles.Add(collectible);

        // Increase collider size
        _collider.radius += collectible.CollectedSize / 10;
        _torque += collectible.CollectedSize * 10;
    }

    private void LoseSomething()
    {
        Collectible detached = _collectibles[_collectibles.Count - 1];
        detached.SetCollected(false);
        _clumpData.DecreaseSize(detached.CollectedSize);
        _collectibles.Remove(detached);

        _collider.radius -= detached.CollectedSize / 10;
        _torque -= detached.CollectedSize * 10;
    }

    private void Update()
    {
        float v = 1f;

        //if (v < 0)
        //{
        //    v *= _reverseSpeedPercentage;
        //}

        //var moveDirection = Camera.main.transform.right * v;

        if (_body.velocity.magnitude < _maxSpeed)
        {
            _body.AddTorque(torqueDirection * _torque * Time.deltaTime, ForceMode.Force);
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
