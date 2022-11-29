using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour
{
    [Header("When this is raised...")]
    [SerializeField] private VoidEventSO _voidEventChannel;

    [Header("...do this.")]
    [SerializeField] private UnityEvent OnEventRaised;

    private void OnEnable()
    {
        _voidEventChannel.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        _voidEventChannel.OnEventRaised -= Respond;
    }

    private void Respond()
    {
        OnEventRaised.Invoke();
    }
}