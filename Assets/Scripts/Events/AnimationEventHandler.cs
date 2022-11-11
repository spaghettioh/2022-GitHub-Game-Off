using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;

    public void AnimationEvent()
    {
        _event.Invoke();
    }
}
