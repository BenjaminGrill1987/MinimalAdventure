using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "MoveAble")]
public class MoveAble : ScriptableObject
{
    [SerializeField] private List<TileBase> moveAbleTiles;
    //[SerializeField] private List<Tile> notMoveAbleTiles;

    public bool CanMove(TileBase tile)
    {
        foreach(TileBase tiles in moveAbleTiles)
        {
            if(tiles == tile)
            {
                return false;
            }
        }
        return true;
    }
}
