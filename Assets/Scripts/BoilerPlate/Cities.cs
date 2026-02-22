using UnityEngine;

public class Cities : Places
{
    private static readonly string[] Prefixes =
    {
        "Neu", "Alt", "Gold", "Eisen", "Stern", "Nord", "Süd"
    };

    private static readonly string[] Suffixes =
    {
        "hafen", "stadt", "heim", "wacht", "burg", "tor"
    };

    public override void HideInterface()
    {
        GameState.TryToChange(GameStates.Game);
        _panel.SetActive(false);
    }

    protected override void GeneratePlaceName()
    {
        _placeName = $"{Prefixes[Random.Range(0, Prefixes.Length)]}{Suffixes[Random.Range(0, Suffixes.Length)]}";
    }
}
