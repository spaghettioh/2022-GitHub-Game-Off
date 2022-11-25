using UnityEngine;
using UnityEngine.Events;

public class PropColliderMesh : MonoBehaviour
{
    public UnityAction<SphereCollider> OnCollision;

    [SerializeField] private SphereCollider _collider;

    public void AdjustSizeAndTrigger(bool isTrigger)
    {
        _collider.isTrigger = isTrigger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (OnCollision != null)
            OnCollision.Invoke((SphereCollider)collision.collider);
        else Debug.LogWarning($"{name} raised a collision but no one listens.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnCollision != null) OnCollision.Invoke((SphereCollider)other);
        else Debug.LogWarning($"{name} raised a trigger but no one listens.");
    }
}
