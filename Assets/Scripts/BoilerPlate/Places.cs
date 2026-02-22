using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Places : MonoBehaviour
{
    private static readonly Dictionary<string, string> GeneratedNamesByGroup = new Dictionary<string, string>();

    [SerializeField] protected TextMeshProUGUI _placeNameSign;
    [SerializeField] protected GameObject _panel, _firstSelected;
    [SerializeField] private string _nameGroupId;

    protected string _placeName;

    protected virtual void Awake()
    {
        string groupKey = ResolveGroupKey();

        if (!GeneratedNamesByGroup.TryGetValue(groupKey, out _placeName))
        {
            GeneratePlaceName();
            GeneratedNamesByGroup[groupKey] = _placeName;
        }

        ApplyPlaceNameToSign();
    }

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

    protected abstract void GeneratePlaceName();

    private void ApplyPlaceNameToSign()
    {
        if (_placeNameSign != null)
        {
            _placeNameSign.text = _placeName;
        }
    }

    private string ResolveGroupKey()
    {
        if (!string.IsNullOrWhiteSpace(_nameGroupId))
        {
            return $"{GetType().Name}:{_nameGroupId}";
        }

        Transform groupRoot = transform.parent != null ? transform.parent : transform;
        return $"{GetType().Name}:auto:{groupRoot.GetInstanceID()}";
    }
}
