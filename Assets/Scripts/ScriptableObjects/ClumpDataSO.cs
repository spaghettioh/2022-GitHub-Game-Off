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

    public void SetUp(Transform t, SphereCollider c, float s)
    {
        Transform = t;
        Collider = c;
        SetSize(s);
    }

    public void SetVelocity(float value)
    {
        Velocity = value;
    }

    public void SetSize(float value)
    {
        Size = value;
        CheckScaleChange();
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
        var currentScale = Scale;

        switch (Mathf.Floor(Size))
        {
            case 0f:
                Scale = PropScaleCategory._00Tiniest;
                break;

            case 1f:
                Scale = PropScaleCategory._01Tiny;
                break;

            case 2f:
                Scale = PropScaleCategory._02VerySmall;
                break;

            case 3f:
                Scale = PropScaleCategory._03Small;
                break;

            case 4f:
                Scale = PropScaleCategory._04BelowAverage;
                break;

            case 5f:
                Scale = PropScaleCategory._05Average;
                break;

            case 6f:
                Scale = PropScaleCategory._06AboveAverage;
                break;

            case 7f:
                Scale = PropScaleCategory._07Large;
                break;

            case 8f:
                Scale = PropScaleCategory._08VeryLarge;
                break;

            case 9f:
                Scale = PropScaleCategory._09Huge;
                break;

            case 10f:
                Scale = PropScaleCategory._10Massive;
                break;

            default:
                break;
        }

        if (Scale != currentScale)
        {
            AnnounceScaleChange();
        }

        AnnounceSizeChange();
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
}
