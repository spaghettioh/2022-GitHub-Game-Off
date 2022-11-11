using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private List<Collectible> _collectibles;
    [SerializeField] private ClumpDataSO clumpDataSO;

    private void Start()
    {
        _collectibles = new List<Collectible>(FindObjectsOfType<Collectible>());
    }
}
