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
        _transform.DOScale(propData.Scale / 2f, 0f);
        _scorePoints = propData.Points;
        _transform.DOMove(position, 0);
        return this;
    }

    private void FixedUpdate()
    {
        if (_body.IsSleeping() && _isFalling)
        {
            _isFalling = false;

            if (OnSleep != null)
            {
                OnSleep.Invoke();
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning($"{name} feel asleep but no one listens.");
            }
#endif
        }
    }

    public void Drop()
    {
        _body.useGravity = true;
        _body.isKinematic = false;
        _body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }
}
