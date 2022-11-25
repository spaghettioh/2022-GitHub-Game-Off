using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public static class ClumpPropCollection
{
    //[SerializeField] private static List<Prop> _clumpPropCollection;
    //[SerializeField] private ClumpPropCollectionSO _clumpPropCollection;

    [field: SerializeField] private static List<Prop> _propsCollected;
    //public List<Prop> PropsCollected { get { return _propsCollected; } }

    public static void Reset()
    {
        _propsCollected = new List<Prop>();
    }

    public static void AddProp(Prop prop)
    {
        Debug.Log(prop);
        _propsCollected.Add(prop);
    }

    public static void RemoveProp(Prop prop)
    {
        _propsCollected.Remove(prop);
    }

    public static void PrepForWinScreen()
    {
        _propsCollected.ForEach(prop =>
        {
            // TODO 
        });
    }

    public static int GetCount()
    {
        return _propsCollected.Count;
    }

    public static Prop GetLast()
    {
        return _propsCollected.Last();
    }

    public static List<Prop> GetProps()
    {
        return _propsCollected;
    }

    public static List<Prop> GetAttachingProps()
    {
        return _propsCollected.FindAll(p => p.IsAttaching);
    }
    //private void Update()
    //{
    //    Debug.Log(_clumpPropCollection.PropsCollected.Count);
    //    _clumpPropCollection.PropsCollected.ForEach(prop =>
    //    Debug.Log(prop.name));
    //}
}
