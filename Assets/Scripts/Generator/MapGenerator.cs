using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private int _cityChance, _smallCities, _mediumCities, _largeCities, _mountainChance, _forestChance;

    [SerializeField] private List<TileBase> _tile;
    [SerializeField] private Tilemap _overWorldMap, _forestMap, _mountainMap, _citiesMap, _pathMap;
    [SerializeField] private Button _startButton;

    [SerializeField] private List<Kernel> _kernel;
    [SerializeField] private int _rounds;

    private int[,] _mapArray;
    private bool _permissionGenerate = false;

    public void StartGenerator()
    {
        if (_permissionGenerate)
        {
            GenerateMap();
            RenumberArray();
            DrawMap();
            GeneratePaths();
            _startButton.interactable = true;
            MapHandler.SetOverworldMap(_mapArray);
        }
        else
        {
            Debug.LogError("You have to choose a size");
        }
    }

    public void StartGame()
    {
        GameState.TryToChange(GameStates.Game);
    }

    private void GenerateMap()
    {
        int width = _mapArray.GetLength(0);
        int height = _mapArray.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_mapArray[x, y] == 0)
                {
                    if (TryPlaceCity(x, y, width, height))
                    {
                        continue;
                    }

                    if (Random.Range(0, 100) < _mountainChance)
                    {
                        _mapArray[x, y] = 1;
                        continue;
                    }

                    if (Random.Range(0, 100) < _forestChance)
                    {
                        _mapArray[x, y] = 3;
                    }
                }
            }
        }
    }

    private bool TryPlaceCity(int x, int y, int width, int height)
    {
        if (Random.Range(0, 100) >= _cityChance)
        {
            return false;
        }

        if (x + 1 >= width || y + 1 >= height)
        {
            _mapArray[x, y] = 2;
            return true;
        }

        _mapArray[x, y] = 2;

        if (Random.Range(0, 100) < _smallCities)
        {
            if (Random.Range(0, 100) < 50)
            {
                _mapArray[x + 1, y] = 2;
            }
            else
            {
                _mapArray[x, y + 1] = 2;
            }

            return true;
        }

        if (Random.Range(0, 100) < _mediumCities)
        {
            int mediumPattern = Random.Range(0, 3);
            if (mediumPattern == 0)
            {
                _mapArray[x, y + 1] = 2;
                _mapArray[x + 1, y] = 2;
            }
            else if (mediumPattern == 1)
            {
                _mapArray[x, y + 1] = 2;
                _mapArray[x + 1, y + 1] = 2;
            }
            else
            {
                _mapArray[x + 1, y] = 2;
                _mapArray[x + 1, y + 1] = 2;
            }

            return true;
        }

        if (Random.Range(0, 100) < _largeCities)
        {
            _mapArray[x + 1, y] = 2;
            _mapArray[x + 1, y + 1] = 2;
            _mapArray[x, y + 1] = 2;
        }

        return true;
    }

    void RenumberArray()
    {
        for (int x = 0; x < _kernel.Count; x++)
        {
            for (int y = 0; y <= _rounds; y++)
            {
                _mapArray = _kernel[x].RestructureMap(_mapArray);
            }
        }
    }

    private void DrawMap()
    {
        int width = _mapArray.GetLength(0);
        int height = _mapArray.GetLength(1);

        BoundsInt mapBounds = new BoundsInt(0, 0, 0, width, height, 1);
        int tileCount = width * height;

        TileBase[] overworldTiles = new TileBase[tileCount];
        TileBase[] mountainTiles = new TileBase[tileCount];
        TileBase[] cityTiles = new TileBase[tileCount];
        TileBase[] forestTiles = new TileBase[tileCount];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x + (y * width);
                overworldTiles[index] = _tile[0];

                switch (_mapArray[x, y])
                {
                    case 1:
                        mountainTiles[index] = _tile[1];
                        break;
                    case 2:
                        cityTiles[index] = _tile[2];
                        break;
                    case 3:
                        forestTiles[index] = _tile[3];
                        break;
                }
            }
        }

        _overWorldMap.ClearAllTiles();
        _mountainMap.ClearAllTiles();
        _citiesMap.ClearAllTiles();
        _forestMap.ClearAllTiles();
        _pathMap.ClearAllTiles();

        _overWorldMap.SetTilesBlock(mapBounds, overworldTiles);
        _mountainMap.SetTilesBlock(mapBounds, mountainTiles);
        _citiesMap.SetTilesBlock(mapBounds, cityTiles);
        _forestMap.SetTilesBlock(mapBounds, forestTiles);
    }

    private void GeneratePaths()
    {
        List<List<Vector2Int>> cityGroups = GetCityGroups();
        List<Vector2Int> largeCityCenters = new List<Vector2Int>();

        foreach (List<Vector2Int> cityGroup in cityGroups)
        {
            if (cityGroup.Count > 1)
            {
                largeCityCenters.Add(GetCityCenter(cityGroup));
            }
        }

        if (largeCityCenters.Count < 2)
        {
            return;
        }

        for (int i = 0; i < largeCityCenters.Count; i++)
        {
            int nearestIndex = GetNearestCityIndex(largeCityCenters, i);
            if (nearestIndex >= 0)
            {
                DrawPath(largeCityCenters[i], largeCityCenters[nearestIndex]);
            }
        }
    }

    private List<List<Vector2Int>> GetCityGroups()
    {
        int width = _mapArray.GetLength(0);
        int height = _mapArray.GetLength(1);
        bool[,] visited = new bool[width, height];
        List<List<Vector2Int>> cityGroups = new List<List<Vector2Int>>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (visited[x, y] || _mapArray[x, y] != 2)
                {
                    continue;
                }

                List<Vector2Int> group = new List<Vector2Int>();
                Queue<Vector2Int> queue = new Queue<Vector2Int>();
                queue.Enqueue(new Vector2Int(x, y));
                visited[x, y] = true;

                while (queue.Count > 0)
                {
                    Vector2Int current = queue.Dequeue();
                    group.Add(current);

                    TryAddCityNeighbor(queue, visited, current.x + 1, current.y, width, height);
                    TryAddCityNeighbor(queue, visited, current.x - 1, current.y, width, height);
                    TryAddCityNeighbor(queue, visited, current.x, current.y + 1, width, height);
                    TryAddCityNeighbor(queue, visited, current.x, current.y - 1, width, height);
                }

                cityGroups.Add(group);
            }
        }

        return cityGroups;
    }

    private void TryAddCityNeighbor(Queue<Vector2Int> queue, bool[,] visited, int x, int y, int width, int height)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return;
        }

        if (!visited[x, y] && _mapArray[x, y] == 2)
        {
            visited[x, y] = true;
            queue.Enqueue(new Vector2Int(x, y));
        }
    }

    private Vector2Int GetCityCenter(List<Vector2Int> cityGroup)
    {
        int sumX = 0;
        int sumY = 0;

        for (int i = 0; i < cityGroup.Count; i++)
        {
            sumX += cityGroup[i].x;
            sumY += cityGroup[i].y;
        }

        return new Vector2Int(sumX / cityGroup.Count, sumY / cityGroup.Count);
    }

    private int GetNearestCityIndex(List<Vector2Int> cityCenters, int cityIndex)
    {
        int nearestIndex = -1;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < cityCenters.Count; i++)
        {
            if (i == cityIndex)
            {
                continue;
            }

            float distance = Vector2Int.Distance(cityCenters[cityIndex], cityCenters[i]);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }

    private void DrawPath(Vector2Int from, Vector2Int to)
    {
        int x = from.x;
        int y = from.y;

        while (x != to.x)
        {
            x += x < to.x ? 1 : -1;
            SetPathTile(x, y);
        }

        while (y != to.y)
        {
            y += y < to.y ? 1 : -1;
            SetPathTile(x, y);
        }
    }

    private void SetPathTile(int x, int y)
    {
        if (_mapArray[x, y] != 2)
        {
            _mapArray[x, y] = 4;
            _pathMap.SetTile(new Vector3Int(x, y), _tile[4]);
        }
    }

    public void MapSmall()
    {
        _mapArray = new int[64, 64];

        _text.text = "Small";

        _permissionGenerate = true;
    }

    public void MapMedium()
    {
        _mapArray = new int[128, 128];

        _text.text = "Medium";

        _permissionGenerate = true;
    }

    public void MapBig()
    {
        _mapArray = new int[256, 256];

        _text.text = "Big";

        _permissionGenerate = true;
    }
}
