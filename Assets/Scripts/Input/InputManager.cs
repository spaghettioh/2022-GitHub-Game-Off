using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputHandlerSO _inputHandler;
    [SerializeField] private TouchInput _touchInput;
    private Vector2 _keys;
    private Vector2 _touch;

    private void Update()
    {
        _keys = new Vector2(Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));

        _touch = new Vector2(_touchInput.Horizontal, _touchInput.Vertical);

        if (_keys != Vector2.zero)
        {
            _inputHandler.SetDirectionalInput(_keys);
        }
        else if (_touch != Vector2.zero)
        {
            _inputHandler.SetDirectionalInput(_touch);
        }
        else
        {
            _inputHandler.SetDirectionalInput(Vector2.zero);
        }
    }
}
