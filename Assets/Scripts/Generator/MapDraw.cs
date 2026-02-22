using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDraw : Singleton<MapDraw>
{
    [SerializeField] Tilemap _overWorldMap, _forestMap, _citiesMap, _mountainMap, _pathMap;
    [SerializeField] List<TileBase> _tile;
    [SerializeField] GameObject _mapEvent;

    public static Tilemap MountainMap { get => Instance._mountainMap; }

    private void Start()
    {
        DrawMap();
    }

    private void DrawMap()
    {
        int[,] overWorld = MapHandler.GetOverworldMap();
        int width = overWorld.GetLength(0);
        int height = overWorld.GetLength(1);
        int tileCount = width * height;

        BoundsInt mapBounds = new BoundsInt(0, 0, 0, width, height, 1);

        TileBase[] overWorldTiles = new TileBase[tileCount];
        TileBase[] mountainTiles = new TileBase[tileCount];
        TileBase[] cityTiles = new TileBase[tileCount];
        TileBase[] forestTiles = new TileBase[tileCount];
        TileBase[] pathTiles = new TileBase[tileCount];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x + (y * width);
                overWorldTiles[index] = _tile[0];

                switch (overWorld[x, y])
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
                    case 4:
                        pathTiles[index] = _tile[4];
                        break;
                }
            }
        }

        _overWorldMap.ClearAllTiles();
        _mountainMap.ClearAllTiles();
        _citiesMap.ClearAllTiles();
        _forestMap.ClearAllTiles();
        _pathMap.ClearAllTiles();

        _overWorldMap.SetTilesBlock(mapBounds, overWorldTiles);
        _mountainMap.SetTilesBlock(mapBounds, mountainTiles);
        _citiesMap.SetTilesBlock(mapBounds, cityTiles);
        _forestMap.SetTilesBlock(mapBounds, forestTiles);
        _pathMap.SetTilesBlock(mapBounds, pathTiles);
    }

    public static TileBase GetOverWorldMapTile(Vector3Int pos)
    {
        return MountainMap.GetTile<TileBase>(pos);
    }
}
