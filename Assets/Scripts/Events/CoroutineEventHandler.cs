using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoroutineEventHandler : MonoBehaviour
{
    [Header("After this many seconds...")]
    [SerializeField] private float _waitTime;

    [Header("...do this:")]
    [SerializeField] private UnityEvent _event;

    private void OnEnable()
    {
        StartCoroutine(WaitTimeRoutine());
    }

    private IEnumerator WaitTimeRoutine()
    {
        yield return new WaitForSeconds(_waitTime);
        _event.Invoke();
    }
}
