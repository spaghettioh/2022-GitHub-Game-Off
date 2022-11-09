using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour
{
	[Header("Listening to...")]
	[SerializeField] private VoidEventSO _voidEventChannel;

	public UnityEvent OnEventRaised;

	private void OnEnable()
	{
		if (_voidEventChannel != null)
			_voidEventChannel.OnEventRaised += Respond;
	}

	private void OnDisable()
	{
		if (_voidEventChannel != null)
			_voidEventChannel.OnEventRaised -= Respond;
	}

	private void Respond()
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke();
	}
}