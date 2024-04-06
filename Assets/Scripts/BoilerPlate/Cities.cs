using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cities : Places
{
    public override void HideInterface()
    {
        GameState.TryToChange(GameStates.Game);
        _panel.SetActive(false);
    }
}
