using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private SpriteRenderer _renderer;
    public Transform Graphic { get { return _renderer.transform; } }

    [field: SerializeField]
    public float Size { get; private set; }

    [field: SerializeField]
    public float ClumpSizeChangeAmount { get; private set; }

    [field: SerializeField]
    public float ClumpRadiusChangeAmount { get; private set; }

    [field: SerializeField]
    public float ClumpTorqueChangeAmount { get; private set; }

    [field: SerializeField]
    public bool IsCollectable { get; private set; }

    [Header("Sound FX")]
    [SerializeField] private AudioCueSO _collectSound;
    [SerializeField] private AudioCueSO _crashSoundSmall;
    [SerializeField] private AudioCueSO _crashSoundLarge;

    [Header("Listening to...")]
    [SerializeField] private ClumpDataSO _clumpData;

    [Header("Broadcasting to...")]
    [SerializeField] private PropCollectEventSO _collectEvent;
    [SerializeField] private AudioEventSO _sfxChannel;
    [SerializeField] private VoidEventSO _crashEvent;

    [Space(60)]
    [Header("DEBUG")]
    [SerializeField] private List<PropColliderMesh> _colliders;
    [SerializeField] private Transform _originalParent;
    [Space]
    [SerializeField] private float _flickerDuration = 2f;
    [SerializeField] private float _shakeDuration = 1f;
    [Space]
    private GameObject _attachPoint;
    [SerializeField] private float _attachDuration = 10f;
    [field: SerializeField] public bool IsAttaching { get; private set; }

    private Transform _transform;

    private void Awake()
    {
        _originalParent = transform.parent;
        TryGetComponent(out _transform);
        BuildColliderList();
    }

    private void OnEnable()
    {
        _colliders.ForEach(c =>
        {
            c.OnTrigger += Collect;
            c.OnCollision += Crash;
        });
    }

    private void OnDisable()
    {
        _colliders.ForEach(c =>
        {
            c.OnTrigger -= Collect;
            c.OnCollision -= Crash;
        });
        _transform.DOKill();
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

    private void Crash(Collision collision)
    {
        if (_clumpData.Velocity >= (_clumpData.MaxSpeed / 2))
        {
            _sfxChannel.RaisePlayback(_crashSoundLarge);
            _crashEvent.Raise(name);

            _renderer.transform.DOShakePosition(
                _shakeDuration, .1f / transform.lossyScale.x);
        }
        else
        {
            _sfxChannel.RaisePlayback(_crashSoundSmall);
        }
    }

    public void Collect(Collider collider)
    {
        if (collider != _clumpData.Collider) return;

        _collectEvent.Raise(this);

        // Disable colliders, turn off collectability, inform manager
        ToggleCollectable(false);
        _colliders.ForEach(c => c.gameObject.SetActive(false));
        _sfxChannel.RaisePlayback(_collectSound);

        CreateAttachPoint(collider);

        // Waits for one second to see if a crash will uncollect it
        _transform.DOLocalMove(_transform.localPosition, 1f)
            .OnComplete(MoveTowardAttachPoint);
    }

    private void CreateAttachPoint(Collider collider)
    {
        // Create an object at the closest point to the collider
        _attachPoint = new GameObject("TempAttachPoint");
        _attachPoint.transform.position =
            collider.ClosestPoint(transform.position);
        _attachPoint.transform.SetParent(collider.transform);
    }

    private void MoveTowardAttachPoint()
    {
        // Then move towards it
        Vector3 endPosition = _attachPoint.transform.localPosition;
        _clumpData.IncreaseTorqueAndCollider(
            ClumpTorqueChangeAmount, ClumpRadiusChangeAmount);
        _transform.DOLocalMove(endPosition, _attachDuration).OnComplete(() =>
        {
            Destroy(_attachPoint);
        });
    }

    public void Uncollect()
    {
        // Stop everything
        StopAllCoroutines();
        _transform.DOKill();

        StartCoroutine(UncollectRoutine());
    }
    private IEnumerator UncollectRoutine()
    {
        // Return to the starting parent
        _transform.SetParent(_originalParent);
        _transform.rotation = Quaternion.identity;

        // Moves to a seemingly random position
        var p = transform.localPosition;
        var endPos = Vector3.zero;
        endPos.x = p.x + Random.Range(-1f, 1f);
        endPos.z = p.z + Random.Range(-1f, 1f);
        _transform.DOPunchScale(_transform.localScale * 1.1f, 1f);
        _transform.DOLocalJump(endPos, 1f, 2, 1f);

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

        // Resets the prop
        _colliders.ForEach(c => c.gameObject.SetActive(true));
        ToggleCollectable(true);

        _clumpData.DecreaseTorqueAndCollider(
            ClumpTorqueChangeAmount, ClumpRadiusChangeAmount);
    }

    private void OnValidate()
    {
        BuildColliderList();
        if (_sprite != null)
        {
            _renderer.sprite = _sprite;
        }
        //name = _sprite.name;
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
