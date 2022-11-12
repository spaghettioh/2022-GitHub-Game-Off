using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PropCollisionEvent",
    menuName = "Game Off/Prop Collision Event")]
public class PropCollisionEventSO : ScriptableObject
{
    public UnityAction<Collectible> OnCollisionRaised;

    public void Raise(Collectible collectible)
    {
        if (OnCollisionRaised != null)
        {
            OnCollisionRaised.Invoke(collectible);
        }
        else
        {
            Debug.LogWarning($"{name} raised but nothing listens...");
        }
    }
}
