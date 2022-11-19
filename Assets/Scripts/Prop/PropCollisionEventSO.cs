using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PropCollisionEvent",
    menuName = "Game Off/Prop Collision Event")]
public class PropCollisionEventSO : ScriptableObject
{
    public UnityAction<Collectible> OnCollisionRaised;
    public UnityAction<Prop> OnPropCollisionRaised;

    public void Raise(Collectible collectible, string elevator = "(Unknown)")
    {
        if (OnCollisionRaised != null)
        {
            OnCollisionRaised.Invoke(collectible);
        }
        else
        {
            Debug.LogWarning(
                $"{elevator} raised {name} but nothing listens...");
        }
    }

    public void Raise(Prop prop, string elevator = "(Unknown)")
    {
        if (OnPropCollisionRaised != null)
        {
            OnPropCollisionRaised.Invoke(prop);
        }
        else
        {
            Debug.LogWarning(
                $"{elevator} raised {name} but nothing listens...");
        }
    }
}
