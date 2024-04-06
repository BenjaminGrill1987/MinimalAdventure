using MinimalAdventure.PlayerInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] MoveAble moveable;

    private Vector3Int _pos;
    private CharacterInput _characterController;
    private InputAction _move;

    private void Awake()
    {
        _characterController = new CharacterInput();
        _pos = new Vector3Int(0, 0, 0);
        transform.position = _pos;
    }

    private void OnEnable()
    {
        _characterController.Enable();
        _move = _characterController.Player.Move;
    }

    private void Update()
    {
        if (GameState.CurrentState == GameStates.Game)
        {
            InputUpdate();
        }
        MoveUpdate();
    }

    private void InputUpdate()
    {
        if (_characterController.Player.Move.WasPressedThisFrame())
        {

            if (_move.ReadValue<Vector2>().y > 0 && moveable.CanMove(MapDraw.GetOverWorldMapTile(new Vector3Int(_pos.x, _pos.y + 1, _pos.z))))
            {
                if (_pos.y < MapHandler.GetOverworldMap().GetLength(1) - 1)
                {
                    _pos.y += 1;
                }
            }
            else if (_move.ReadValue<Vector2>().y < 0 && moveable.CanMove(MapDraw.GetOverWorldMapTile(new Vector3Int(_pos.x, _pos.y - 1, _pos.z))))
            {
                if (_pos.y > 0)
                {
                    _pos.y -= 1;
                }
            }
            else if (_move.ReadValue<Vector2>().x > 0 && moveable.CanMove(MapDraw.GetOverWorldMapTile(new Vector3Int(_pos.x + 1, _pos.y, _pos.z))))
            {
                if (_pos.x < MapHandler.GetOverworldMap().GetLength(0) - 1)
                {
                    _pos.x += 1;
                }
            }
            else if (_move.ReadValue<Vector2>().x < 0 && moveable.CanMove(MapDraw.GetOverWorldMapTile(new Vector3Int(_pos.x - 1, _pos.y, _pos.z))))
            {
                if (_pos.x > 0)
                {
                    _pos.x -= 1;
                }
            }
        }
    }

    private void MoveUpdate()
    {
        if (transform.position != _pos)
        {
            transform.position = _pos;
        }
    }
}