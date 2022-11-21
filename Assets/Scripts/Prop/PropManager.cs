using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [Header("Collection config")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private List<Prop> _props;
    [SerializeField] private List<Prop> _collectableProps;
    public List<Prop> CurrentCollection = new List<Prop>();
    [SerializeField] private TransformAnchorSO _clumpPropCollection;

    [Header("Listening to...")]
    [SerializeField] private PropCollectEventSO _propCollectEvent;
    [SerializeField] private VoidEventSO _crashEvent;

    [Header("Editor")]
    [SerializeField] private bool _forceSpriteOrientation;

    private void OnEnable()
    {
        _clumpData.OnSizeChanged += AdjustPropsCollectable;
        _propCollectEvent.OnEventRaised += CollectProp;
        _crashEvent.OnEventRaised += CrashIntoProp;
    }

    private void OnDisable()
    {
        _clumpData.OnSizeChanged -= AdjustPropsCollectable;
        _propCollectEvent.OnEventRaised -= CollectProp;
        _crashEvent.OnEventRaised -= CrashIntoProp;
    }

    private void Start()
    {
        BuildPropsList();
        AdjustPropsCollectable();
    }

    private void BuildPropsList()
    {
        _props = new List<Prop>(GetComponentsInChildren<Prop>());
    }

    private void CollectProp(Prop p)
    {
        CurrentCollection.Add(p);
        _collectableProps.Remove(p);
        p.transform.SetParent(_clumpPropCollection.Transform);
        _clumpData.IncreaseSize(p.ClumpSizeChangeAmount);
    }

    private void CrashIntoProp()
    {
        if (CurrentCollection.Count > 0)
        {
            var attaching = CurrentCollection.FindAll(p => p.IsAttaching);
            if (attaching.Count > 0)
            {
                attaching.ForEach(p =>
                {
                    UncollectProp(p);
                });
            }
            else
            {
                UncollectProp(CurrentCollection.Last());
            }
        }
    }

    private void UncollectProp(Prop p)
    {
        CurrentCollection.Remove(p);
        _collectableProps.Add(p);
        _clumpData.DecreaseSize(p.ClumpSizeChangeAmount);
        p.Uncollect();
    }

    private void AdjustPropsCollectable(float clumpSize = default)
    {
        if (clumpSize == default) clumpSize = _clumpData.Size;

        _props.FindAll(p => p.Size <= clumpSize).ForEach(p =>
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

                var t = p.Graphic.rotation.eulerAngles;
                t.x = 0;
                t.z = 0;
                p.GetComponentInChildren<PropColliderMesh>()
                    .transform.rotation = Quaternion.Euler(t);
            });
        }
    }
}
