using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] private ClumpPropCollectionSO _propCollection;
    [SerializeField] private VoidEventSO _allPropsDropped;
    [SerializeField] private WinScreenPropPoolSO _propPool;
    [SerializeField] private Transform _propParent;
    [SerializeField] private Transform _bottleLeft;
    [SerializeField] private Transform _bottleRight;
    [SerializeField] private List<WinScreenProp> _spawnedProps;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _dropTime;

    [Header("Starts when...")]
    [SerializeField] private VoidEventSO _curtainsOpened;

    [Header("DEBUG / TESTING")]
    [SerializeField] private List<PropData> _propsCollected;
    //[SerializeField] private List<Prop> _testProps;
    [SerializeField] private int _poolSize;

    private void OnEnable()
    {
        _curtainsOpened.OnEventRaised += StartWinScreen;
    }

    private void OnDisable()
    {
        _curtainsOpened.OnEventRaised -= StartWinScreen;
    }

    private void StartWinScreen()
    {
        // Use test data when no props are present - useful for testing
        if (_propCollection.GetPropsWon().Count == 0)
        {
            GetComponentsInChildren<Prop>(true).ToList()
                .ForEach(prop => _propsCollected.Add(
                new PropData(prop.Sprite, prop.transform.localScale.x)));
        }
        else  _propsCollected = _propCollection.GetPropsWon();

        _poolSize = _propsCollected.Count();
        _spawnedProps = _propPool.PreWarm(_poolSize, _propParent);
        PrepProps();
        StartCoroutine(DropProps());
    }

    private void PrepProps()
    {
        for (int i = 0; i < _spawnedProps.Count; i++)
        {
            var spawnedProp = _spawnedProps[i];
            var matchingPropData = _propsCollected[i];
            spawnedProp.Initialize(matchingPropData, GetRandomPosition());
        }
    }

    private Vector3 GetRandomPosition()
    {
        var randomXPos = Random.Range(
            _bottleLeft.position.x, _bottleRight.position.x);
        var randomYPos = Random.Range(
            _bottleLeft.position.y, _bottleLeft.position.y + 2);
        return new Vector3(randomXPos, randomYPos, 0f);
    }

    private IEnumerator DropProps()
    {
        var waitTime = _dropTime / _spawnedProps.Count;
        while (_spawnedProps.Count > 0)
        {
            int randomPropIndex = Random.Range(0, _spawnedProps.Count);
            WinScreenProp prop = _spawnedProps[randomPropIndex];
            _spawnedProps.Remove(prop);

            prop.gameObject.SetActive(true);
            prop.Drop();

            yield return new WaitForSeconds(waitTime);
        }

        PropsFinishedDropping();
    }

    private void PropsFinishedDropping() =>
        StartCoroutine(PropsFinishedDroppingRoutine());
    private IEnumerator PropsFinishedDroppingRoutine()
    {
        yield return new WaitForSeconds(_dropTime);
        _allPropsDropped.Raise(name);
    }
}
