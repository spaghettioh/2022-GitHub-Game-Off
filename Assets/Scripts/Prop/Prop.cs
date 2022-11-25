using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Prop : MonoBehaviour
{
    public UnityAction<Prop> OnCollisionEvent;

    [Header("Config")]
    [SerializeField] private Sprite _sprite;
    public Sprite Sprite
    {
        get { return _renderer.sprite; }
    }
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private TrailRenderer _uncollectTrail;
    [SerializeField] private Transform _transform;

    [Header("Collection")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private AudioCueSO _propCollectSound;
    [field: SerializeField]
    public bool IsCollectable { get; private set; }
    [field: SerializeField] public float SizeToCollect { get; private set; }

    [Space(60)]
    [Header("DEBUG")]
    [SerializeField] private List<PropColliderMesh> _colliders;
    [SerializeField] private Transform _originalParent;
    [Space]
    [SerializeField] private float _flickerDuration = 2f;
    [SerializeField] private float _shakeDuration = 1f;
    [Space]
    [SerializeField] private GameObject _attachPoint;
    [SerializeField] private float _attachDuration = 10f;
    [field: SerializeField] public bool IsAttaching { get; private set; }


    private void Awake()
    {
        _originalParent = transform.parent;
        TryGetComponent(out _transform);
        BuildColliderList();
        SetTrailActive(false);
    }

    private void OnEnable()
    {
        _colliders.ForEach(c =>
        {
            c.OnCollision += RaiseCollision;
        });
    }

    private void OnDisable()
    {
        _colliders.ForEach(c =>
        {
            c.OnCollision -= RaiseCollision;
        });
        _transform.DOKill();
    }

    private void RaiseCollision(Collider collider)
    {
        if (OnCollisionEvent != null) OnCollisionEvent.Invoke(this);
        else Debug.LogWarning($"{name} raised collision but no one listens.");
    }

    private void BuildColliderList()
    {
        _colliders = new List<PropColliderMesh>(
            GetComponentsInChildren<PropColliderMesh>());
    }

    private PropColliderMesh AddColliderScript(GameObject g)
    {
        var collider = g.GetComponent<PropColliderMesh>();
        if (collider == null)
        {
            collider = g.AddComponent<PropColliderMesh>();
        }
        return collider;
    }

    public void ToggleCollectable(bool onOff)
    {
        _colliders.ForEach(c => c.AdjustSizeAndTrigger(onOff));
        IsCollectable = onOff;
    }

    public void ShakeGraphic(float shakeDuration)
    {
        _renderer.transform.DOShakePosition(
            shakeDuration, .1f / transform.lossyScale.x);
    }

    public void SetCollected(AudioEventSO audioEvent)
    {
        // Disable colliders, turn off collectability, inform manager
        ToggleCollectable(false);
        _colliders.ForEach(c => c.gameObject.SetActive(false));

        if (_propCollectSound != null)
            audioEvent.RaisePlayback(_propCollectSound);

        CreateAttachPoint();

        // Waits for one second to see if a crash will uncollect it
        _transform.DOLocalMove(_transform.localPosition, 1f)
            .OnComplete(MoveTowardAttachPoint);
    }

    private void CreateAttachPoint()
    {
        // Create an object at the closest point to the collider
        _attachPoint = new GameObject("PropAttachPoint");
        _attachPoint.transform.position =
            _clumpData.Collider.ClosestPoint(transform.position);
        _attachPoint.transform.SetParent(_clumpData.Transform);
        transform.SetParent(_attachPoint.transform);
    }

    private void MoveTowardAttachPoint()
    {
        // Then move towards it
        _transform.DOLocalMove(Vector3.zero, _attachDuration).OnComplete(() =>
        {
            transform.SetParent(_clumpData.Transform);
            Destroy(_attachPoint);
        });
    }

    public void Uncollect()
    {
        // Stop everything, like moving, or waiting
        StopAllCoroutines();
        _transform.DOKill();

        // Return to the starting parent
        _transform.SetParent(_originalParent);
        _transform.rotation = Quaternion.identity;

        // Moves to a seemingly random position
        SetTrailActive(true);
        var localPos = transform.localPosition;
        var endPos = Vector3.zero;
        endPos.x = localPos.x + Random.Range(-2f, 2f);
        endPos.z = localPos.z + Random.Range(-2f, 2f);
        //_transform.DOPunchScale(_transform.localScale * .5f, 1f, 1, 1f);
        _transform.DOLocalJump(endPos, 1f, 2, 1f).OnComplete(() =>
        {
            StartCoroutine(FlickerRoutine());
        });
    }

    private IEnumerator FlickerRoutine()
    {
        // Flickers the sprite for some time
        var flickerTime = 0f;
        var alphaChange = Color.white;
        while (flickerTime < _flickerDuration)
        {
            alphaChange.a = alphaChange.a == 1 ? 0 : 1;
            _renderer.color = alphaChange;
            var waitTime = Time.deltaTime + .1f;
            yield return new WaitForSeconds(waitTime);
            flickerTime += waitTime;
        }
        _renderer.color = Color.white;
        SetTrailActive(false);

        // Resets the prop
        _colliders.ForEach(c => c.gameObject.SetActive(true));
        ToggleCollectable(true);
    }

    private void SetTrailActive(bool active) =>
        _uncollectTrail.gameObject.SetActive(active);

    private void OnValidate()
    {
        BuildColliderList();
        if (_sprite != null)
        {
            _renderer.sprite = _sprite;
            name = _sprite.name;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;

        Color color;
        color = Color.green;
        color.a = .5f;
        Gizmos.color = color;
        Gizmos.DrawLine(pos + (Vector3.down / 2), pos + (Vector3.up / 2));

        color = Color.red;
        color.a = .5f;
        Gizmos.color = color;
        Gizmos.DrawLine(pos + (Vector3.left / 2), pos + (Vector3.right / 2));

        color = Color.blue;
        color.a = .5f;
        Gizmos.color = color;
        Gizmos.DrawLine(pos + (Vector3.forward / 2), pos + (Vector3.back / 2));
    }
}
