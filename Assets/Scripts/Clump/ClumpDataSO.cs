using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ClumpData", menuName = "Game Off/ClumpData")]
public class ClumpDataSO : ScriptableObject
{
    public UnityAction<int> OnPropCountChanged;
    public UnityAction<float> OnStatsChanged;

    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public float MinColliderRadius { get; private set; }
    [field: SerializeField] public float MaxColliderRadius { get; private set; }
    [field: SerializeField] public float MinMoveForce { get; private set; }
    [field: SerializeField] public float MaxMoveForce { get; private set; }

    [Header("DEBUG ==========")]
    [SerializeField] private string _header;
    [field: SerializeField] public SphereCollider Collider { get; private set; }
    [field: SerializeField] public Transform Transform { get; private set; }
    [field: SerializeField] public int CollectedCount { get; private set; }
    [field: SerializeField] public float Velocity { get; private set; }
    [field: SerializeField] public float MoveForce { get; private set; }

    public void ConfigureData(Transform t, SphereCollider c)
    {
        Transform = t;
        Collider = c;

        MoveForce = MinMoveForce;
        Collider.radius = MinColliderRadius;
        CollectedCount = 0;
    }

    public void IncreaseSize(float radius, float force)
    {
        MoveForce += force;
        Collider.radius += radius;
        CollectedCount++;
        RaiseStatsChange();
    }

    public void DecreaseSize(float radius, float force)
    {
        MoveForce -= force;
        Collider.radius -= radius;
        CollectedCount--;
        RaiseStatsChange();
    }

    public void SetVelocity(float value) => Velocity = value;

    private void RaiseStatsChange()
    {
        if (OnPropCountChanged != null)
        {
            OnPropCountChanged.Invoke(CollectedCount);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{name} announced a size change" +
              $" by no one listens.");
        }
#endif

        if (OnStatsChanged != null)
        {
            OnStatsChanged.Invoke(MoveForce);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{name} announced a size change" +
              $" by no one listens.");
        }
#endif
    }
}
