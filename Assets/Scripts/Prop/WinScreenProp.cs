using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

public class WinScreenProp : MonoBehaviour
{
    public UnityAction OnSleep;

    [SerializeField] private Rigidbody _body;
    [SerializeField] private Transform _transform;
    [SerializeField] private SphereCollider _normalCollider;
    [SerializeField] private SphereCollider _winScreenCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private bool _isFalling;

    private void FixedUpdate()
    {
        if (_body.IsSleeping() && _isFalling)
        {
            _isFalling = false;

            if (OnSleep != null)
                OnSleep.Invoke();
            else
                Debug.LogWarning($"{name} feel asleep but no one listens.");
        }
    }

    public WinScreenProp Initialize(PropData propData, Vector3 position)
    {
        _spriteRenderer.sprite = propData.Sprite;
        _transform.DOScale(propData.Scale / 2f, 0f);
        _transform.DOMove(position, 0);
        _normalCollider.gameObject.SetActive(false);
        _winScreenCollider.radius = _normalCollider.radius / 2f;
        _winScreenCollider.gameObject.SetActive(true);
        return this;
    }

    public void Drop()
    {
        _body.useGravity = true;
        _body.isKinematic = false;
        _body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }
}
