using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Places : MonoBehaviour
{
    private static readonly Dictionary<string, string> GeneratedNamesByGroup = new Dictionary<string, string>();
    private static readonly Dictionary<int, int[,]> ComponentMapsByTileValue = new Dictionary<int, int[,]>();
    private static int[,] _cachedMapReference;

    [SerializeField] protected TextMeshProUGUI _placeNameSign;
    [SerializeField] protected GameObject _panel, _firstSelected;
    [SerializeField] private GossipGenerator _gossipGenerator;
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
        ResolveGossipGenerator();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameState.CurrentState == GameStates.Game)
        {
            _panel.SetActive(true);
            _gossipGenerator?.GenerateGossip(_placeName);
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


    private void ResolveGossipGenerator()
    {
        if (_gossipGenerator != null || _panel == null)
        {
            return;
        }

        _gossipGenerator = _panel.GetComponentInChildren<GossipGenerator>(true);
    }


    public static string GetRandomOtherPlaceName(string currentPlaceName)
    {
        List<string> knownPlaceNames = GeneratedNamesByGroup
            .Values
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct()
            .Where(name => !string.Equals(name, currentPlaceName, System.StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (knownPlaceNames.Count == 0)
        {
            return null;
        }

        return knownPlaceNames[Random.Range(0, knownPlaceNames.Count)];
    }

    private string ResolveGroupKey()
    {
        if (!string.IsNullOrWhiteSpace(_nameGroupId))
        {
            return $"{GetType().Name}:{_nameGroupId}";
        }

        if (TryGetMapGroupKey(out string mapGroupKey))
        {
            return mapGroupKey;
        }

        Vector3 position = transform.position;
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        return $"{GetType().Name}:tile:{x}:{y}";
    }

    private bool TryGetMapGroupKey(out string key)
    {
        key = default;
        int[,] map = MapHandler.GetOverworldMap();
        if (map == null)
        {
            return false;
        }

        if (!TryGetTileCoordinate(map, out int x, out int y))
        {
            return false;
        }

        int targetTileValue = GetGroupingTileValue();
        if (map[x, y] != targetTileValue)
        {
            return false;
        }

        RefreshComponentCacheIfNeeded(map);

        if (!ComponentMapsByTileValue.TryGetValue(targetTileValue, out int[,] componentMap))
        {
            componentMap = BuildComponentMap(map, targetTileValue);
            ComponentMapsByTileValue[targetTileValue] = componentMap;
        }

        int componentId = componentMap[x, y];
        if (componentId <= 0)
        {
            return false;
        }

        key = $"{GetType().Name}:map:{targetTileValue}:{componentId}";
        return true;
    }

    private bool TryGetTileCoordinate(int[,] map, out int x, out int y)
    {
        Vector3 position = transform.position;
        x = Mathf.RoundToInt(position.x);
        y = Mathf.RoundToInt(position.y);

        int width = map.GetLength(0);
        int height = map.GetLength(1);
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    protected virtual int GetGroupingTileValue()
    {
        return 2;
    }

    private void RefreshComponentCacheIfNeeded(int[,] map)
    {
        if (ReferenceEquals(_cachedMapReference, map))
        {
            return;
        }

        _cachedMapReference = map;
        ComponentMapsByTileValue.Clear();
    }

    private int[,] BuildComponentMap(int[,] map, int targetValue)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        int[,] componentMap = new int[width, height];
        int componentId = 0;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] != targetValue || componentMap[x, y] != 0)
                {
                    continue;
                }

                componentId++;
                componentMap[x, y] = componentId;
                queue.Enqueue(new Vector2Int(x, y));

                while (queue.Count > 0)
                {
                    Vector2Int current = queue.Dequeue();

                    TryVisitNeighbor(map, componentMap, targetValue, componentId, queue, current.x + 1, current.y, width, height);
                    TryVisitNeighbor(map, componentMap, targetValue, componentId, queue, current.x - 1, current.y, width, height);
                    TryVisitNeighbor(map, componentMap, targetValue, componentId, queue, current.x, current.y + 1, width, height);
                    TryVisitNeighbor(map, componentMap, targetValue, componentId, queue, current.x, current.y - 1, width, height);
                }
            }
        }

        return componentMap;
    }

    private void TryVisitNeighbor(int[,] map, int[,] componentMap, int targetValue, int componentId, Queue<Vector2Int> queue, int x, int y, int width, int height)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return;
        }

        if (map[x, y] != targetValue || componentMap[x, y] != 0)
        {
            return;
        }

        componentMap[x, y] = componentId;
        queue.Enqueue(new Vector2Int(x, y));
    }
}
