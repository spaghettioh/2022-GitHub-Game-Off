using UnityEngine;
using UnityEngine.Events;

public class PropColliderMesh : MonoBehaviour
{
    public UnityAction<Collider> OnCollision;

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
        if (OnCollision != null) OnCollision.Invoke(collision.collider);
        else Debug.LogWarning($"{name} raised a collision but no one listens.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnCollision != null) OnCollision.Invoke(other);
        else Debug.LogWarning($"{name} raised a trigger but no one listens.");
    }
}
