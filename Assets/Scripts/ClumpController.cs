using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClumpController : MonoBehaviour
{
    [SerializeField] private float _torque = 2f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private TransformAnchorSO _clumpTransformAnchor;

    [SerializeField] private float _startingMass;
    [SerializeField] private ClumpMassSO _clumpMass;

    [SerializeField] private CollectEventSO _collectEvent;
    private List<Collectible> _collectibles = new List<Collectible>();

    private Rigidbody _body;

    private void OnEnable()
    {
        _collectEvent.OnCollected = CollectSomething;
    }

    private void OnDisable()
    {
        _collectEvent.OnCollected -= CollectSomething;
    }

    private void CollectSomething(Collectible collectible)
    {
        _clumpMass.Increase(collectible.Mass);
        _collectibles.Add(collectible);
    }

    private void LoseSomething()
    {
        // TODO Drop the last collected thing
    }

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        _clumpTransformAnchor.Set(transform);
        _clumpMass.Set(_startingMass);
    }

    private void Update()
    {
        var v = Camera.main.transform.right * Input.GetAxisRaw("Vertical");

        if (_body.velocity.magnitude < _maxSpeed)
        {
            _body.AddTorque(v * _torque, ForceMode.Force);
        }
    }


}
