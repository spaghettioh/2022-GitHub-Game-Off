using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClumpController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _torque = 2f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _reverseModifier = .66f;

    [Header("Mass")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private float _startingMass;
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

    private void OnEnable()
    {
        _collectEvent.OnCollected = CollectSomething;
        //_knockEvent.OnEventRaised = LoseSomething;
    }

    private void OnDisable()
    {
        _collectEvent.OnCollected -= CollectSomething;
        _knockEvent.OnEventRaised -= LoseSomething;
    }

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        _clumpData.SetTransform(transform);
        _clumpData.SetSize(_startingMass);
        TryGetComponent(out _collider);
    }

    private void CollectSomething(Collectible collectible)
    {
        var currentMass = _clumpData.Mass;

        _clumpData.IncreaseSize(collectible.Mass * _collectionModifier);
        _collectibles.Add(collectible);

        // Increase collider size
        if (_clumpData.Mass >= Mathf.Ceil(currentMass))
        {
            _collider.radius += _colliderChange;
            _torque += _colliderChange * 100f;
        }
    }

    private void LoseSomething()
    {
        Collectible detached = _collectibles[_collectibles.Count - 1];
        detached.SetCollected(false);
        _clumpData.DecreaseSize(detached.Mass * _collectionModifier);
        _collectibles.Remove(detached);

        //_collider.radius -= detached.Mass;
        //_torque -= detached.Mass;
    }

    private void Update()
    {
        var v = Input.GetAxisRaw("Vertical");
        if (v < 0)
        {
            v *= _reverseModifier;
        }

        var moveDirection = Camera.main.transform.right * v;

        if (_body.velocity.magnitude < _maxSpeed)
        {
            _body.AddTorque(moveDirection * _torque * Time.deltaTime, ForceMode.Force);
        }
    }


}
