using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ClumpData", menuName = "Game Off/ClumpData")]
public class ClumpDataSO : ScriptableObject
{
    public UnityAction<float> OnSizeChanged;
    public UnityAction<PropScaleCategory> OnScaleChanged;

    [SerializeField] private ClumpScaleConfigSO _scaleConfigs;

    public Transform Transform { get; private set; }
    public float Size { get; private set; }
    public SphereCollider Collider { get; private set; }
    public float Velocity { get; private set; }
    public float Torque { get; private set; }
    [field: SerializeField] public float MaxSpeed { get; private set; }

    [Tooltip("Percentage of max speed to move in reverse")]
    [field: SerializeField]
    public float ReverseSpeedPercentage { get; private set; }

    public PropScaleCategory Scale { get; private set; }

    private float _startSize;
    private PropScaleCategory _startScale;

    public void SetUp(Transform t, SphereCollider c, float s)
    {
        Transform = t;
        Collider = c;

        Size = s;
        _startSize = s;

        Scale = GetCurrentScale();
        _startScale = Scale;

        // Clump config
        Collider.radius = _scaleConfigs.GetConfig(Scale).ColliderRadius;
        Torque = _scaleConfigs.GetConfig(Scale).Torque;
    }

    public void IncreaseTorqueAndCollider(float value)
        => IncreaseTorqueAndCollider(value, value);
    public void IncreaseTorqueAndCollider(float torque, float radius)
    {
        Torque += torque;
        Collider.radius += radius;
    }

    public void DecreaseTorqueAndCollider(float value)
        => IncreaseTorqueAndCollider(value, value);
    public void DecreaseTorqueAndCollider(float torque, float radius)
    {
        Torque -= torque;
        Collider.radius -= radius;
    }

    public void SetVelocity(float value) => Velocity = value;

    public void IncreaseSize(float value)
    {
        Size += value;
        CheckForChanges();
    }

    public void DecreaseSize(float value)
    {
        Size -= value;
        CheckForChanges();
    }

    private void CheckForChanges()
    {
        var peviousScale = Scale;
        var currentScale = GetCurrentScale();

        if (currentScale != peviousScale)
        {
            Scale = currentScale;
            Torque = _scaleConfigs.GetConfig(Scale).Torque;
            Collider.radius = _scaleConfigs.GetConfig(Scale).ColliderRadius;
            AnnounceScaleChange();
        }

        AnnounceSizeChange();
    }

    private PropScaleCategory GetCurrentScale()
    {
        return _scaleConfigs.GetConfig(Mathf.Floor(Size)).PropScale;
    }

    private void AnnounceScaleChange()
    {
        if (OnScaleChanged != null) OnScaleChanged.Invoke(Scale);
        else Debug.LogWarning($"Nobody heard scale change event from {name}");
    }

    private void AnnounceSizeChange()
    {
        if (OnSizeChanged != null) OnSizeChanged.Invoke(Size);
        else Debug.LogWarning($"Nobody heard size change event from {name}");
    }

    private void OnDisable()
    {
        Size = _startSize;
        Scale = _startScale;
    }
}
