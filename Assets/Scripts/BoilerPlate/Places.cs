using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Places : MonoBehaviour
{
    [SerializeField] protected GameObject _panel, _firstSelected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameState.CurrentState == GameStates.Game)
        {
            _panel.SetActive(true);
            GameState.TryToChange(GameStates.Places);
            EventSystem.current.SetSelectedGameObject(_firstSelected);
        }
    }

    public abstract void HideInterface();
}
