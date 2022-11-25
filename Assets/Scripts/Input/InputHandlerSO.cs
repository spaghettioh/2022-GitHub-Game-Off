﻿using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu()]
public class InputHandlerSO : ScriptableObject
{
    public UnityAction<Vector2> OnDirectionalInput;
    public Vector2 DirectionalInput;

    public void SetDirectionalInput(Vector2 input, string elevator = "Unknown")
    {
        if (OnDirectionalInput != null)
            OnDirectionalInput.Invoke(input);

        DirectionalInput = input;
    }
}
    