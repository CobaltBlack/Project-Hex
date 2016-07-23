using UnityEngine;
using System.Collections;

public class CombatBoardManager : MonoBehaviour {

    public MapHex[,] gameBoard;

    // Use this for initialization
    void SetupBoard (CombatParameters parameters) {
	
	}

    // Returns array of all tiles within given range
    public HexTile[] GetSurroundingTiles(int range)
    {
        HexTile[] adjacentTiles = { };
        return adjacentTiles;
    }

    // Returns true if other HexTile is adjacent to current
    public bool IsAdjacentTo(HexTile other)
    {
        return true;
    }
}
