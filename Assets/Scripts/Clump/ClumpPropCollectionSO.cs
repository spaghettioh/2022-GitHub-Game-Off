using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ClumpPropCollection"
    , menuName = "Game Off/Clump prop collection")]
public class ClumpPropCollectionSO : ScriptableObject
{
    [SerializeField] private PropCollectEventSO _collectEvent;
    [SerializeField] private List<Prop> _propsCollected;

    public void Reset()
    {
        _propsCollected = new List<Prop>();
    }

    public void AddProp(Prop prop)
    {
        _propsCollected.Add(prop);
    }

    public void RemoveProp(Prop prop)
    {
        _propsCollected.Remove(prop);
    }

    public void PrepForWinScreen()
    {
        _propsCollected.ForEach(prop =>
        {
            // TODO 
        });
    }

    public int GetCount()
    {
        return _propsCollected.Count;
    }

    public Prop GetLast()
    {
        return _propsCollected.Last();
    }

    public List<Prop> GetAttachingProps()
    {
        return _propsCollected.FindAll(p => p.IsAttaching);
    }
}
