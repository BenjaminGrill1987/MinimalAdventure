using TMPro;
using UnityEngine;

public class GossipGenerator : MonoBehaviour
{
    [SerializeField] private Transform _gossipContent;
    [SerializeField] private GameObject _gossipPanelPrefab;
    [SerializeField] private string _gossipPanelResourcePath = "Prefabs/GossipPanel";
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

    [SerializeField] private string[] _directionHints =
    {
        "nördlich von hier",
        "südlich von hier",
        "östlich von hier",
        "westlich von hier",
        "hinter den alten Hügeln"
    };

    private void Awake()
    {
        if (_gossipPanelPrefab == null && !string.IsNullOrWhiteSpace(_gossipPanelResourcePath))
        {
            _gossipPanelPrefab = Resources.Load<GameObject>(_gossipPanelResourcePath);
        }
    }

    public void GenerateGossip(string placeName)
    {
        if (_gossipPanelPrefab == null && !string.IsNullOrWhiteSpace(_gossipPanelResourcePath))
        {
            _gossipPanelPrefab = Resources.Load<GameObject>(_gossipPanelResourcePath);
        }

        if (_gossipContent == null || _gossipPanelPrefab == null || _gossipTemplates.Length == 0)
        {
            return;
        }

        ClearCurrentGossip();

        int entriesToCreate = Mathf.Max(1, _gossipEntriesCount);

        for (int i = 0; i < entriesToCreate; i++)
        {
            Object created = Instantiate((Object)_gossipPanelPrefab, _gossipContent);
            GameObject gossipPanel = (created as GameObject) ?? (created as Component)?.gameObject;

            if (gossipPanel == null)
            {
                continue;
            }

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
        string otherPlaceName = Places.GetRandomOtherPlaceName(placeName);
        string directionHint = _directionHints.Length > 0
            ? _directionHints[Random.Range(0, _directionHints.Length)]
            : "in der Ferne";

        string placeReplacement = string.IsNullOrWhiteSpace(otherPlaceName)
            ? directionHint
            : otherPlaceName;

        return template
            .Replace("{place}", placeReplacement)
            .Replace("{subject}", subject)
            .Replace("{direction}", directionHint);
    }

    private void ClearCurrentGossip()
    {
        for (int i = _gossipContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_gossipContent.GetChild(i).gameObject);
        }
    }
}
