using UnityEngine;
using UnityEngine.Events;

public class PropColliderMesh : MonoBehaviour
{
    public UnityAction<Collision> OnCollision;
    public UnityAction<Collider> OnTrigger;

    private Collider _collider;

    private void Awake()
    {
        TryGetComponent(out _collider);
    }

    public void AdjustSizeAndTrigger(bool isTrigger)
    {
        _collider.isTrigger = isTrigger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (OnCollision != null) OnCollision.Invoke(collision);
        else Debug.LogWarning($"{name} raised a collision but no one listens.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnTrigger != null) OnTrigger.Invoke(other);
        else Debug.LogWarning($"{name} raised a trigger but no one listens.");
    }
}
