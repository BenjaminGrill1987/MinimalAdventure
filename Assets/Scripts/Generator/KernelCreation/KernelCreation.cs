using NaughtyAttributes;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KernelCreation : MonoBehaviour
{
    [BoxGroup("File Input")]
    [SerializeField] private TextAsset cSV;
    [SerializeField] private string patternName;
    [SerializeField] private Tilemap patternMap, replacementPatternMap;
    [SerializeField] private bool patternOverride;

    [SerializeField] private List<TileBase> tileList;

    private int[,] patternArray, replacementPatternArray;
    private int row, column;


    [Button]
    private void SavePattern()
    {
        SaveArray();
        string path = Application.dataPath + "/Resources/CSVFiles/" + cSV.name + ".csv";
        StreamWriter writer;
        if (!patternOverride)
        {
            writer = new StreamWriter(path, true);
        }
        else
        {
            writer = new StreamWriter(path, false);
        }

        writer.WriteLine(patternName);
        writer.WriteLine(row.ToString() + "," + column.ToString());

        for(int x = 0;x<row;x++)
        {
            string newPattern = "";
            for(int y = 0;y<column;y++)
            {
                if(y==0)
                {
                    newPattern = patternArray[x, y].ToString();
                }
                else
                {
                    newPattern = newPattern + "," + patternArray[x, y].ToString();
                }
            }
            writer.WriteLine(newPattern);
        }

        for (int x = 0; x < row; x++)
        {
            string newPattern = "";
            for (int y = 0; y < column; y++)
            {
                if (y == 0)
                {
                    newPattern = replacementPatternArray[x, y].ToString();
                }
                else
                {
                    newPattern = newPattern + "," + replacementPatternArray[x, y].ToString();
                }
            }
            writer.WriteLine(newPattern);
        }
        writer.WriteLine();
        writer.Close();
        Debug.LogError("DONE");
        DeleteMaps();
    }

    private void SaveArray()
    {
        row = patternMap.cellBounds.xMax;
        column = patternMap.cellBounds.yMax;
        patternArray = new int[row,column];
        replacementPatternArray = new int[row, column];
        SaveMap(patternMap, patternArray);
        SaveMap(replacementPatternMap, replacementPatternArray);
        
    }

    private void SaveMap(Tilemap tilemap, int[,] array)
    {
        Vector3Int pos = new Vector3Int(0, 0, 0);
        for (int x = 0;x < row;x++)
        {
            for(int y = 0;y< column;y++)
            {
                string tile = tilemap.GetTile(pos).ToString();

                for(int i = 0;i<tileList.Count;i++)
                {
                    if(tile == tileList[i].ToString())
                    {
                        array[x, y] = i;
                        break;
                    }
                }
                pos = new Vector3Int(pos.x, pos.y + 1, 0);
            }
            pos = new Vector3Int(pos.x + 1, 0, 0);
        }
    }

    [Button]
    private void DeleteMaps()
    {
        patternMap.ClearAllTiles();
        replacementPatternMap.ClearAllTiles();
    }
}
