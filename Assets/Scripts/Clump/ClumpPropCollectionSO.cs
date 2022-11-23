using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ClumpPropCollection"
    , menuName = "Game Off/Clump prop collection")]
public class ClumpPropCollectionSO : ScriptableObject
{
    [SerializeField] private PropCollectEventSO _collectEvent;

    private void OnEnable()
    {
        Debug.Log("Clump collection SO enable");
    }
}
