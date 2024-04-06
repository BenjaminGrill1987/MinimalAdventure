using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : Singleton<MapHandler>
{
    private int[,] overWorldMap;

    public static int[,] OverWorldMap { get => Instance.overWorldMap;}

    public static void SetOverworldMap(int[,] map)
    {
        Instance.overWorldMap = map;
    }

    public static int[,] GetOverworldMap()
    {
        return OverWorldMap;
    }
}