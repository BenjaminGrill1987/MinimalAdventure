using MinimalAdventure.PlayerInput;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private CharacterInput _characterInput;
    private InputAction _moveAction;

    public Vector2 Move => _moveAction.ReadValue<Vector2>();

    private void Awake()
    {
        _characterInput = new CharacterInput();
        _moveAction = _characterInput.Player.Move;
    }

    private void OnEnable()
    {
        _characterInput.Enable();
    }

    private void OnDisable()
    {
        _characterInput.Disable();
    }

    public bool MoveWasPressedThisFrame()
    {
        return _moveAction.WasPressedThisFrame();
    }
}
