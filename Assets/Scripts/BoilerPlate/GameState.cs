using UnityEngine;

public class GameState : Singleton<GameState>
{
    private GameStates currentState;
    private SceneHandler sceneHandler;

    public static GameStates CurrentState { get => Instance.currentState; }

    private void Start()
    {
        sceneHandler = new SceneHandler();
        currentState = GameStates.Menu;
    }

    public static void TryToChange(GameStates changeTo)
    {
        if (CurrentState == changeTo)
        {
            Debug.LogError("You are in state " + changeTo.ToString());
            return;
        }

        switch (changeTo)
        {
            case GameStates.Menu:
                {
                    ChangeCurrentState(changeTo);
                    break;
                }
            case GameStates.MapGenerator:
                {
                    ChangeCurrentState(changeTo);
                    LoadScene("MapGenerator");
                    break;
                }
            case GameStates.Game:
                {
                    if (CurrentState != GameStates.Places)
                    {
                        LoadScene("Game");
                    }
                    ChangeCurrentState(changeTo);
                    break;
                }
            case GameStates.Places:
                {
                    ChangeCurrentState(changeTo);
                    break;
                }
            case GameStates.Quit:
                {
                    ChangeCurrentState(changeTo);
                    Application.Quit();
                    break;
                }
        }
    }

    private static void ChangeCurrentState(GameStates states)
    {
        Instance.currentState = states;
    }

    private static void LoadScene(string sceneName)
    {
        Instance.sceneHandler.LoadScene(sceneName);
    }
}

public enum GameStates
{
    Menu,
    MapGenerator,
    Game,
    Places,
    Quit
}