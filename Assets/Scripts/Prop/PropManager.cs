using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [Header("Collecting")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private ClumpPropCollectionSO _propCollection;
    [SerializeField] private AudioCueSO _defaultCollectSound;

    [Header("Crashing")]
    [SerializeField] private float _crashShakeDuration;
    [SerializeField] private AudioCueSO _crashSoundSmall;
    [SerializeField] private AudioCueSO _crashSoundLarge;

    [Header("Listening to...")]
    [SerializeField] private PropCollectEventSO _propCollectEvent;

    [Header("Broadcasting to...")]
    [SerializeField] private AudioEventSO _audioEvent;
    [SerializeField] private VoidEventSO _crashEvent;

    [Header("Editor")]
    [SerializeField] private bool _forceSpriteOrientation;

    [Header("DEBUG ==========")]
    [SerializeField] private float _clumpRadiusIncrement;
    [SerializeField] private float _clumpForceIncrement;
    [SerializeField] private List<Prop> _props;
    [SerializeField] private List<Prop> _collectableProps;

    private void OnEnable()
    {
        _clumpData.OnPropCountChanged += AdjustPropsCollectable;
        _propCollectEvent.OnEventRaised += CollectProp;
    }

    private void OnDisable()
    {
        _clumpData.OnPropCountChanged -= AdjustPropsCollectable;
        _propCollectEvent.OnEventRaised -= CollectProp;
        _props.ForEach(prop => prop.OnCollisionEvent -= ProcessCollision);
    }

    private void Start()
    {
        _propCollection.Reset();
        BuildPropsList();
        _props.ForEach(prop => prop.OnCollisionEvent += ProcessCollision);
        SetClumpIncrements();
        AdjustPropsCollectable();
    }

    private void ProcessCollision(Prop prop)
    {
        Debug.Log($"Process collisiosn {prop.IsCollectable}");

        if (prop.IsCollectable)
        {
            CollectProp(prop);
        }
        else
        {
            CrashIntoProp(prop);
        }
    }

    private void BuildPropsList()
    {
        _props = new List<Prop>(GetComponentsInChildren<Prop>());
    }

    private void SetClumpIncrements()
    {
        var minCollider = _clumpData.MinColliderRadius;
        var maxCollider = _clumpData.MaxColliderRadius;
        _clumpRadiusIncrement = (maxCollider - minCollider) / _props.Count();

        var minForce = _clumpData.MinMoveForce;
        var maxForce = _clumpData.MaxMoveForce;
        _clumpForceIncrement = (maxForce - minForce) / _props.Count();
    }

    private void CollectProp(Prop p)
    {
        _audioEvent.RaisePlayback(_defaultCollectSound);
        _propCollection.AddProp(p);
        _collectableProps.Remove(p);
        p.SetCollected(_audioEvent);
        _clumpData.IncreaseSize(_clumpRadiusIncrement, _clumpForceIncrement);
    }

    private void CrashIntoProp(Prop p)
    {
        Debug.Log($"Crash velocity: {_clumpData.Velocity}");
        if (_clumpData.Velocity >= _clumpData.MaxSpeed / 3)
        {
            _audioEvent.RaisePlayback(_crashSoundLarge);

            // TODO make this query return all attaching or just the last one
            if (_propCollection.GetCount() > 0)
            {
                var attaching = _propCollection.GetAttachingProps();
                if (attaching.Count > 0)
                {
                    attaching.ForEach(p => UncollectProp(p));
                }
                else
                {
                    UncollectProp(_propCollection.GetLast());
                }
            }
        }
        else
        {
            _audioEvent.RaisePlayback(_crashSoundSmall);
        }

        p.ShakeGraphic(_crashShakeDuration);
        _crashEvent.Raise(name);
    }

    private void UncollectProp(Prop p)
    {
        _propCollection.RemoveProp(p);
        _collectableProps.Add(p);
        _clumpData.DecreaseSize(_clumpRadiusIncrement, _clumpForceIncrement);
        p.Uncollect();
    }

    private void AdjustPropsCollectable(int count = 0)
    {
        _props.FindAll(p => p.SizeToCollect <= _clumpData.CollectedCount)
            .ForEach(p =>
        {
            _props.Remove(p);
            _collectableProps.Add(p);
            p.ToggleCollectable(true);
        });
    }

    private void OnValidate()
    {
        BuildPropsList();

        if (_forceSpriteOrientation)
        {
            _props.ForEach(p =>
            {
                p.GetComponentInChildren<Billboard>().OrientNow();
                _forceSpriteOrientation = false;

                //var t = p.Graphic.rotation.eulerAngles;
                //t.x = 0;
                //t.z = 0;
                //p.GetComponentInChildren<PropColliderMesh>()
                //    .transform.rotation = Quaternion.Euler(t);
            });
        }
    }
}
