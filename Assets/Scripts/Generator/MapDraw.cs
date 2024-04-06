using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDraw : Singleton<MapDraw>
{
    [SerializeField] Tilemap _overWorldMap, _forestMap, _citiesMap, _mountainMap;
    [SerializeField] List<TileBase> _tile;

    public static Tilemap MountainMap { get => Instance._mountainMap; }

    private void Start()
    {
        DrawMap();
    }

    private void DrawMap()
    {
        var pos = new Vector3Int(0, 0, 0);

        for (int x = 0; x < MapHandler.GetOverworldMap().GetLength(0); x++)
        {
            for (int y = 0; y < MapHandler.GetOverworldMap().GetLength(1); y++)
            {
                pos = new Vector3Int(x, y, 0);
                _overWorldMap.SetTile(pos, _tile[0]);
                switch (MapHandler.GetOverworldMap()[x, y])
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

    public static TileBase GetOverWorldMapTile(Vector3Int pos)
    {
        return MountainMap.GetTile<TileBase>(pos);
    }
}
