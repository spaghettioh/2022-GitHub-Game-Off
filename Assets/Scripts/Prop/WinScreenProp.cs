using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class WinScreenProp : MonoBehaviour
{
    public UnityAction OnSleep;

    [SerializeField] private Rigidbody _body;
    [SerializeField] private Transform _transform;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private bool _isFalling;
    [SerializeField] private int _scorePoints;
    public int ScorePoints { get { return _scorePoints; } }

    public WinScreenProp Initialize(PropData propData, Vector3 position)
    {
        _spriteRenderer.sprite = propData.Sprite;
        _transform.localScale = Vector3.one * (propData.Scale / 2f);
        _scorePoints = propData.Points;
        _transform.position = position;
        return this;
    }

    private void FixedUpdate()
    {
        if (_body.velocity.magnitude < .1f && _isFalling)
        {
            _body.Sleep();
            _body.isKinematic = true;
            _body.useGravity = false;
            _isFalling = false;
        }
    }

    public void Drop() => StartCoroutine(DropRoutine());
    private IEnumerator DropRoutine()
    {
        _body.useGravity = true;
        _body.isKinematic = false;
        _body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        // Allow the prop to fall for a second
        yield return new WaitForSeconds(2);
        _isFalling = true;
    }
}
