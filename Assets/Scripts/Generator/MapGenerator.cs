using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private int _cityChance, _smallCities, _mediumCities, _largeCities, _mountainChance, _forestChance;

    [SerializeField] private List<TileBase> _tile;
    [SerializeField] private Tilemap _overWorldMap, _forestMap, _mountainMap, _citiesMap;
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
        for (int x = 0; x < _mapArray.GetLength(0); x++)
        {
            for (int y = 0; y < _mapArray.GetLength(1); y++)
            {
                if (_mapArray[x, y] == 0)
                {
                    bool setPoint = false;

                    if (Random.Range(0, 100) < _cityChance)
                    {
                        if (x + 1 != _mapArray.GetLength(0) && y + 1 != _mapArray.GetLength(1))
                        {
                            bool cityBuild = false;
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
                                cityBuild = true;
                            }
                            if (Random.Range(0, 100) < _mediumCities && !cityBuild)
                            {
                                if (Random.Range(0, 100) < 25)
                                {
                                    _mapArray[x, y + 1] = 2;
                                    _mapArray[x + 1, y] = 2;
                                    cityBuild = true;
                                }
                                if (Random.Range(0, 100) < 25 && !cityBuild)
                                {
                                    _mapArray[x, y + 1] = 2;
                                    _mapArray[x, y + 1] = 2;
                                    cityBuild = true;
                                }
                                if (!cityBuild)
                                {
                                    _mapArray[x + 1, y] = 2;
                                    _mapArray[x + 1, y + 1] = 2;
                                    cityBuild = true;
                                }
                            }
                            if (Random.Range(0, 100) < _largeCities && !cityBuild)
                            {
                                _mapArray[x + 1, y] = 2;
                                _mapArray[x + 1, y + 1] = 2;
                                _mapArray[x, y + 1] = 2;
                            }

                        }
                        setPoint = true;
                    }
                    if (Random.Range(0, 100) < _mountainChance && !setPoint)
                    {
                        _mapArray[x, y] = 1;
                        setPoint = true;
                    }
                    if (Random.Range(0, 100) < _forestChance && !setPoint)
                    {
                        _mapArray[x, y] = 3;
                        setPoint = true;
                    }
                    if (!setPoint)
                    {
                        _mapArray[x, y] = 0;
                    }
                }
            }
        }
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
        Vector3Int pos = new Vector3Int(0, 0);

        for (int x = 0; x < _mapArray.GetLength(0); x++)
        {
            for (int y = 0; y < _mapArray.GetLength(1); y++)
            {
                pos = new Vector3Int(x, y);
                _overWorldMap.SetTile(pos, _tile[0]);
                switch (_mapArray[x, y])
                {
                    case 1:
                        {
                            _mountainMap.SetTile(pos, _tile[1]);
                            break;
                        }
                    case 2:
                        {
                            _citiesMap.SetTile(pos, _tile[2]);
                            break;
                        }
                    case 3:
                        {
                            _forestMap.SetTile(pos, _tile[3]);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
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