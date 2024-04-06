using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameState.TryToChange(GameStates.MapGenerator);
    }

    public void Options()
    {

    }

    public void ExitGame()
    {
        GameState.TryToChange(GameStates.Quit);
    }
}
