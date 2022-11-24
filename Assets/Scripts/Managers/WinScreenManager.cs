using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] private ClumpPropCollectionSO _propCollection;
    [SerializeField] private VoidEventSO _allPropsSettled;
    [SerializeField] private PropPoolSO _propPool;
    [SerializeField] private int _poolSize;
    [SerializeField] private Transform _propParent;

    [Header("Starts when...")]
    [SerializeField] private VoidEventSO _curtainsOpened;

    private void Start()
    {
        //_propCollection.
        _propPool.PreWarm(_poolSize, _propParent);
    }
}
