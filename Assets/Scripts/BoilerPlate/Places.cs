using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Places : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _placeNameSign;
    [SerializeField] protected GameObject _panel, _firstSelected;

    protected string _placeName;

    protected virtual void Awake()
    {
        GeneratePlaceName();
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
}
