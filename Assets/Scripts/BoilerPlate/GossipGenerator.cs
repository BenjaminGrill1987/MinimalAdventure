using TMPro;
using UnityEngine;

public class GossipGenerator : MonoBehaviour
{
    [SerializeField] private TMP_Text _gossipText;
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

    private void Awake()
    {
        if (_gossipText == null)
        {
            _gossipText = GetComponent<TMP_Text>();
        }
    }

    public void GenerateGossip(string placeName)
    {
        if (_gossipText == null || _gossipTemplates.Length == 0)
        {
            return;
        }

        string template = _gossipTemplates[Random.Range(0, _gossipTemplates.Length)];
        string subject = _subjects.Length > 0 ? _subjects[Random.Range(0, _subjects.Length)] : "etwas Seltsames";

        string gossip = template
            .Replace("{place}", placeName)
            .Replace("{subject}", subject);

        _gossipText.text = gossip;
    }
}
