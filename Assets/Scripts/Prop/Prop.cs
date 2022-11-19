using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Linq;

public class Prop : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Transform _graphic;
    private SpriteRenderer _renderer;
    
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

    private Transform _t;

    private void Awake()
    {
        _colliders = new List<PropColliderMesh>();
        GetComponentsInChildren<MeshCollider>().ToList()
            .ForEach(m => _colliders.Add(
                m.gameObject.AddComponent<PropColliderMesh>()));
        _originalParent = transform.parent;
        _graphic.TryGetComponent(out _renderer);
        TryGetComponent(out _t);
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
        _t.DOKill();
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

            _graphic.DOShakePosition(_shakeDuration, .1f);
        }
        else
        {
            _sfxChannel.RaisePlayback(_crashSoundSmall);
        }
    }

    public void Collect(Collider collider)
    {
        if (collider != _clumpData.Collider) return;

        // Disable colliders, turn off collectability, inform manager
        ToggleCollectable(false);
        _colliders.ForEach(c => c.gameObject.SetActive(false));
        _sfxChannel.RaisePlayback(_collectSound);

        // Create an object at the closest point to the collider
        _attachPoint = new GameObject();
        _attachPoint.transform.position =
            collider.ClosestPoint(transform.position);
        _attachPoint.transform.SetParent(collider.transform);
        _attachPoint.name = "TempAttachPoint";

        // Then move towards it
        Vector3 endPosition = _attachPoint.transform.localPosition;
        StartCoroutine(CollectRoutine());
        _t.DOLocalMove(endPosition, _attachDuration).OnComplete(() =>
        {
            Destroy(_attachPoint);
        });

        _clumpData.IncreaseTorqueAndCollider(
            ClumpTorqueChangeAmount, ClumpRadiusChangeAmount);
        _collectEvent.Raise(this);
    }

    private IEnumerator CollectRoutine()
    {
        IsAttaching = true;
        yield return new WaitForSeconds(1f);
        IsAttaching = false;
    }

    public void Uncollect() => StartCoroutine(UncollectRoutine());
    private IEnumerator UncollectRoutine()
    {
        // Stop everything
        _t.DOComplete();
        // Return to the starting parent
        _t.SetParent(_originalParent);
        _t.rotation = Quaternion.identity;

        // Moves to a seemingly random position
        var p = transform.localPosition;
        var randomX = p.x + UnityEngine.Random.Range(-1f, 1f);
        var randomZ = p.z + UnityEngine.Random.Range(-1f, 1f);
        var endPos = new Vector3(randomX, 0, randomZ);
        _t.DOPunchScale(_t.localScale * 1.1f, 1f);
        _t.DOLocalJump(endPos, 1f, 2, 1f);

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
        GetComponentInChildren<SpriteRenderer>().sprite = _sprite;
    }
}
