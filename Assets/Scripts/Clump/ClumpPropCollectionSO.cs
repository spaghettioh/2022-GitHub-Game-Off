using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ClumpPropCollection"
    , menuName = "Game Off/Clump prop collection")]
public class ClumpPropCollectionSO : ScriptableObject
{
    [SerializeField] private VoidEventSO _winEvent;
    [SerializeField] private List<Prop> _currentScenePropsCollected;
    [SerializeField] private List<PropData> _propsForWinScreen;

    private void OnEnable()
    {
        _winEvent.OnEventRaised += BuildWonPropData;
    }

    private void OnDisable()
    {
        _winEvent.OnEventRaised -= BuildWonPropData;
    }

    public void Reset()
    {
        _currentScenePropsCollected = new List<Prop>();
        _propsForWinScreen = new List<PropData>();
    }

    public void AddProp(Prop prop)
    {
        _currentScenePropsCollected.Add(prop);
    }

    public void RemoveProp(Prop prop)
    {
        _currentScenePropsCollected.Remove(prop);
    }

    // TODO - All 3 of these are only used to know
    // if PropManager can find an attaching
    // prop, so just move this and that check into that method
    public int GetCount()
    {
        return _currentScenePropsCollected.Count;
    }
    public Prop GetLast()
    {
        return _currentScenePropsCollected.Last();
    }
    public List<Prop> GetAttachingProps()
    {
        return _currentScenePropsCollected.FindAll(p => p.IsAttaching);
    }

    // Waits until the scene is won before trying to build out prop data
    // to avoid having to manage the list in each method
    private void BuildWonPropData()
    {
        _currentScenePropsCollected.ForEach(prop => _propsForWinScreen.Add(
            new PropData(prop.Sprite, prop.transform.localScale.x)));
    }

    public List<PropData> GetPropsWon()
    {
        return _propsForWinScreen;
    }
}
