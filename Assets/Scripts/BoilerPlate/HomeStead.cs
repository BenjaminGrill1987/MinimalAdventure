using UnityEngine;

public class HomeStead : Places
{
    private static readonly string[] FirstParts =
    {
        "Mühlen", "Wiesen", "Birken", "Sonnen", "Bach", "Apfel", "Heide"
    };

    private static readonly string[] LastParts =
    {
        "hof", "feld", "grund", "au", "hain", "tal"
    };

    public override void HideInterface()
    {
        GameState.TryToChange(GameStates.Game);
        _panel.SetActive(false);
    }

    protected override void GeneratePlaceName()
    {
        _placeName = $"{FirstParts[Random.Range(0, FirstParts.Length)]}{LastParts[Random.Range(0, LastParts.Length)]}";
    }
}
