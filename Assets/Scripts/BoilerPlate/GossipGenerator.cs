using TMPro;
using UnityEngine;

public class GossipGenerator : MonoBehaviour
{
    [SerializeField] private Transform _gossipContent;
    [SerializeField] private GameObject _gossipPanelPrefab;
    [SerializeField] private int _gossipEntriesCount = 4;
    [SerializeField] private string[] _gossipTemplates =
    {
        "In {place} sagt man, dass heute Nacht ein Geheimgang geöffnet wird.",
        "Zwischen den Tavernen von {place} kursiert das Gerücht über einen verlorenen Ring.",
        "Ein Händler schwört, dass bei {place} {subject} gesichtet wurde.",
        "Die Leute in {place} flüstern, dass der Bürgermeister ein Doppelleben führt.",
        "Vor den Toren von {place} soll bald ein großes Turnier stattfinden.",
        "Man erzählt, dass {subject} den Markt in {place} unsicher macht.",
        "Im Schatten von {place} wurden alte Runen entdeckt.",
        "Ein Reisender behauptet, in {place} gebe es eine Karte zu vergessenen Ruinen."
    };

    [SerializeField] private string[] _subjects =
    {
        "eine maskierte Diebesbande",
        "ein wandernder Alchemist",
        "eine goldene Kutsche ohne Fahrer",
        "ein Wolf mit leuchtenden Augen",
        "eine verschollene Ritterin"
    };

    public void GenerateGossip(string placeName)
    {
        if (_gossipContent == null || _gossipPanelPrefab == null || _gossipTemplates.Length == 0)
        {
            return;
        }

        ClearCurrentGossip();

        int entriesToCreate = Mathf.Max(1, _gossipEntriesCount);

        for (int i = 0; i < entriesToCreate; i++)
        {
            GameObject gossipPanel = Instantiate(_gossipPanelPrefab, _gossipContent);
            TMP_Text gossipText = gossipPanel.GetComponentInChildren<TMP_Text>(true);

            if (gossipText == null)
            {
                continue;
            }

            gossipText.text = BuildGossip(placeName);
        }
    }

    private string BuildGossip(string placeName)
    {
        string template = _gossipTemplates[Random.Range(0, _gossipTemplates.Length)];
        string subject = _subjects.Length > 0 ? _subjects[Random.Range(0, _subjects.Length)] : "etwas Seltsames";

        return template
            .Replace("{place}", placeName)
            .Replace("{subject}", subject);
    }

    private void ClearCurrentGossip()
    {
        for (int i = _gossipContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_gossipContent.GetChild(i).gameObject);
        }
    }
}
