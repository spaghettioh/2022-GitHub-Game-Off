using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour
{
	[SerializeField] private bool _testNow;

	[Header("Listening to...")]
	[SerializeField] private VoidEventSO _voidEventChannel;

	[SerializeField] private UnityEvent OnEventRaised;

	private void OnEnable() => _voidEventChannel.OnEventRaised += Respond;

	private void OnDisable() => _voidEventChannel.OnEventRaised -= Respond;

	private void Respond() => OnEventRaised.Invoke();
}