using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputHandlerSO _inputHandler;
    [SerializeField] private TouchInput _westJoystick;
    [SerializeField] private TouchInput _eastJoystick;

    [Header("DEBUG ==========")]
    [SerializeField] private Vector2 _keys;
    [SerializeField] private Vector2 _westTouch;
    [SerializeField] private Vector2 _eastTouch;

    private void Update()
    {
        _keys = new Vector2(
            Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        _westTouch = new Vector2(
            _westJoystick.Horizontal, _westJoystick.Vertical);

        _eastTouch = new Vector2(
            _eastJoystick.Horizontal, _eastJoystick.Vertical);

        if (_keys != Vector2.zero)
        {
            _inputHandler.SetDirectionalInput(_keys);
        }
        else if (_westTouch != Vector2.zero)
        {
            _inputHandler.SetDirectionalInput(_westTouch);
        }
        else if (_eastTouch != Vector2.zero)
        {
            _inputHandler.SetDirectionalInput(_eastTouch);
        }
        else
        {
            _inputHandler.SetDirectionalInput(Vector2.zero);
        }
    }
}
