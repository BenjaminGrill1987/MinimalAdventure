using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Kernel : ScriptableObject
{
    int[,] map;
    int[,] replacementPattern;
    int[,] pattern;
    Dictionary<string, int[,]> patterns = new Dictionary<string, int[,]>();
    Dictionary<string, int[,]> replacmentPatterns = new Dictionary<string, int[,]>();

    [ShowNonSerializedField]
    int row, column;

    string patternName;

    [BoxGroup("File Input")]
    [SerializeField] protected TextAsset cSV;

    /// <summary>
    /// Function to Restructure map arrays the function returns the map array after it checked every grid
    /// </summary>
    /// <param name="map"> array from the generator</param>
    /// <returns>returns the map array</returns>
    public int[,] RestructureMap(int[,] map)
    {
        this.map = map;
        CSVReader();
        foreach (string key in patterns.Keys)
        {
            pattern = patterns[key];
            replacementPattern = replacmentPatterns[key];
            row = pattern.GetLength(0);
            column = pattern.GetLength(1);
            ReNumberMap();
        }
        return this.map;
    }

    /// <summary>
    /// The map array will be checked with a grid if the numbers equals the grid numbers. 
    /// When they are the same the numbers in the map array will be changed to the numbers in the partmap array
    /// </summary>
    void ReNumberMap()
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                bool IsEqual = false;
                for (int checkY = 0; checkY < column; checkY++)
                {
                    for (int checkX = 0; checkX < row; checkX++)
                    {
                        if (map.GetLength(0) > x + checkX && map.GetLength(1) > y + checkY)
                        {
                            if (map[x + checkX, y + checkY] == pattern[checkX, checkY])
                            {
                                IsEqual = true;
                            }
                            else
                            {
                                IsEqual = false;
                                break;
                            }
                        }
                        else
                        {
                            IsEqual = false;
                            break;
                        }
                    }
                    if (!IsEqual)
                    {
                        break;
                    }
                    else if (IsEqual && column == checkY + 1)
                    {
                        for (int replaceY = 0; replaceY < column; replaceY++)
                        {
                            for (int replaceX = 0; replaceX < row; replaceX++)
                            {
                                if (map[x + replaceX, y + replaceY] != replacementPattern[replaceX, replaceY])
                                {
                                    map[x + replaceX, y + replaceY] = replacementPattern[replaceX, replaceY];
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Read the CSV Data and get the rules for the partcheck and partmap arrays
    /// </summary>
    void CSVReader()
    {
        //Save all the lines in a array
        string[] lines = cSV.text.Split("\n"[0]);
        //We check in which level we are and adding the new stats to the enemy
        int index = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (index == 0)
            {
                string[] parts = lines[i].Split(","[0]);
                patternName = parts[0];
                index++;
            }
            else if (index == 1)
            {
                string[] parts = lines[i].Split(","[0]);
                row = int.Parse(parts[0]);
                column = int.Parse(parts[1]);
                pattern = new int[row, column];
                replacementPattern = new int[row, column];
                index++;
            }
            else if (index > 1 && index <= row + 1)
            {
                string[] parts = lines[i].Split(","[0]);

                for (int j = 0; j < column; j++)
                {
                    pattern[index - 2, j] = int.Parse(parts[j]);
                }
                index++;
            }
            else if (index > row + 1 && index <= (row + row + 1))
            {
                string[] parts = lines[i].Split(","[0]);

                for (int j = 0; j < column; j++)
                {
                    replacementPattern[index - row - 2, j] = int.Parse(parts[j]);
                }
                index++;
            }
            else if (index > (row + row + 1))
            {
                index = 0;
                if (!patterns.ContainsKey(patternName))
                {
                    patterns.Add(patternName, pattern);
                }
                if (!replacmentPatterns.ContainsKey(patternName))
                {
                    replacmentPatterns.Add(patternName, replacementPattern);
                }
            }
        }
    }
}