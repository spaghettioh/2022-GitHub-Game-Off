using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ClumpData", menuName = "Game Off/ClumpData")]
public class ClumpDataSO : ScriptableObject
{
    public UnityAction<float> OnSizeChanged;
    public UnityAction<PropScaleCategory> OnScaleChanged;

    public Transform Transform { get; private set; }
    public float Size { get; private set; }
    public SphereCollider Collider { get; private set; }
    public float Velocity { get; private set; }
    [Tooltip("Current default = 70")]
    [field: SerializeField] public float Torque { get; private set; }
    [Tooltip("Current default = 7")]
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
    }

    public void SetVelocity(float value)
    {
        Velocity = value;
    }

    public void IncreaseSize(float value)
    {
        Size += value;
        CheckScaleChange();
    }

    public void DecreaseSize(float value)
    {
        Size -= value;
        CheckScaleChange();
    }

    private void CheckScaleChange()
    {
        var peviousScale = Scale;
        var currentScale = GetCurrentScale();

        if (currentScale != peviousScale)
        {
            Scale = currentScale;
            AnnounceScaleChange();
        }

        AnnounceSizeChange();
    }

    private PropScaleCategory GetCurrentScale()
    {
        switch (Mathf.Floor(Size))
        {
            case 0f: return PropScaleCategory._00Tiniest;
            case 1f: return PropScaleCategory._01Tiny;
            case 2f: return PropScaleCategory._02VerySmall;
            case 3f: return PropScaleCategory._03Small;
            case 4f: return PropScaleCategory._04BelowAverage;
            case 5f: return PropScaleCategory._05Average;
            case 6f: return PropScaleCategory._06AboveAverage;
            case 7f: return PropScaleCategory._07Large;
            case 8f: return PropScaleCategory._08VeryLarge;
            case 9f: return PropScaleCategory._09Huge;
            case 10f: return PropScaleCategory._10Massive;
            default: return PropScaleCategory._02VerySmall;
        }
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
