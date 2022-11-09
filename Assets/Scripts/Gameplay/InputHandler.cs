using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    public UnityAction<Vector2> OnDirectionalInput;

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(h, v);

        if (OnDirectionalInput != null)
        {
            OnDirectionalInput.Invoke(input);
        }
        else
        {
            Debug.LogWarning($"No one hear input event from {name}");
        }
    }
}
