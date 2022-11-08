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
    [SerializeField] private float _startingMass;
    [SerializeField] private ClumpMassSO _clumpMass;
    [Tooltip("Collected mass percentage added to clump")]
    [SerializeField] private float _rollUpChange = .1f;

    [Header("Collection")]
    [SerializeField] private CollectEventSO _collectEvent;
    [SerializeField] private VoidEventSO _knockEvent;
    [SerializeField] private List<Collectible> _collectibles = new List<Collectible>();
    [SerializeField] private float _collectionModifier;



    [Header("Broadcasting to...")]
    [SerializeField] private TransformAnchorSO _clumpTransformAnchor;

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
        _clumpTransformAnchor.Set(transform);
        _clumpMass.Set(_startingMass);
        TryGetComponent(out _collider);
    }

    private void CollectSomething(Collectible collectible)
    {
        _clumpMass.Increase(collectible.Mass * _collectionModifier);
        _collectibles.Add(collectible);

        if (_clumpMass.Mass >= Mathf.Ceil(_clumpMass.Mass))
        {
            _collider.radius += _collectionModifier;
        }
        //_torque += collectible.Mass;
    }

    private void LoseSomething()
    {
        Collectible detached = _collectibles[_collectibles.Count - 1];
        detached.SetCollected(false);
        _clumpMass.Decrease(detached.Mass * _collectionModifier);
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
