using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CollectEvent", menuName = "Game Off/Collect Event")]
public class CollectEventSO : ScriptableObject
{
    public UnityAction<Collectible> OnCollected;

    public void Raise(Collectible collectible)
    {
        if (OnCollected != null)
        {
            OnCollected.Invoke(collectible);
        }
        else
        {
            Debug.LogWarning("Nothing heard Collect Event");
        }
    }
}
