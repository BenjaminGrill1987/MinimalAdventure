using UnityEngine;
using UnityEngine.EventSystems;

public class HomeStead : Places
{
    public override void HideInterface()
    {
        GameState.TryToChange(GameStates.Game);
        _panel.SetActive(false);
    }
}
