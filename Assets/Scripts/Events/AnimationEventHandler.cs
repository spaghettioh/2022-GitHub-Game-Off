using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent _onAnimationEvent;

    public void AnimationEvent()
    {
        _onAnimationEvent.Invoke();
    }
}
