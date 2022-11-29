using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
    fileName = "InputHandler", menuName = "Game Off/Input handler")]
public class InputHandlerSO : ScriptableObject
{
    public UnityAction<Vector2> OnDirectionalInput;
    public Vector2 DirectionalInput;

    public void SetDirectionalInput(Vector2 input, string elevator = "Unknown")
    {
        if (OnDirectionalInput != null)
        {
            OnDirectionalInput.Invoke(input);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{elevator} tried to set directional input " +
                $"but no one listens.");
        }
#endif

        DirectionalInput = input;
    }
}
