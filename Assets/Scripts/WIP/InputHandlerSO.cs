using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class InputHandlerSO : ScriptableObject
{
    public Vector2 DirectionalInput { get; private set; }

    public void SetDirectionalInput(Vector2 input)
    {
        DirectionalInput = input.normalized;
    }
}
    