using UnityEngine;
using System.Collections;

/*
 * HexOverlayManager
 * 
 * This script controls the clickable overlays that receives input from the player
 *  
 */

public class HexOverlayManager : MonoBehaviour
{
    GameObject overlayHolder;

    // Instantiates a clickable overlay for each tile given
    public void InstantiateOverlays(HexTile[] tiles)
    {
        overlayHolder = new GameObject("HexOverlays");
        for (int i = 0; i < tiles.Length; i++)
        {
            GameObject overlayObj = InstantiateOverlay(tiles[i]);
            overlayObj.transform.SetParent(overlayHolder.transform);
        }
    }

    public void RemoveAllOverlays()
    {
        Destroy(overlayHolder);
    }

    // Instantiates and returns an overlay for the given HexTile
    GameObject InstantiateOverlay(HexTile tile)
    {
        // Instantiate gameObject
        GameObject instance = Instantiate(hexOverlay, tile.position, Quaternion.identity) as GameObject;

        // Set some attributes
        instance.name = "hexOverlay";
        HexOverlay overlayScript = instance.GetComponent<HexOverlay>();
        overlayScript.x = tile.x;
        overlayScript.y = tile.y;
        return instance;
    }

    //==========================
    // OLD IMPLEMENTATION ======
    //==========================
    public GameObject hexOverlay;

    BoardManager boardScript;

    Transform hexHolderTransform;
    HexOverlay hexScriptTop;
    HexOverlay hexScriptBottom;
    HexOverlay hexScriptTopLeft;
    HexOverlay hexScriptTopRight;
    HexOverlay hexScriptBottomLeft;
    HexOverlay hexScriptBottomRight;

    public void moveOverlay(int x, int y)
    {
        hexHolderTransform.position = boardScript.GetHexPosition(x, y);
        updateHexInfo(x, y);
    }

    public void destroyOverlay()
    {
        //GameObject toDestroy = GameObject.Find("Overlay");
        Destroy(hexHolderTransform.gameObject);
    }

    // Instantiates 6 clickable hex overlays around coordinates xy
    // Each overlay is attached to a parent GameObject hexHolder
    // We move hexHolder to move all 6 overlays at the same time
    public void InstantiateOverlayAroundPlayer(int x, int y)
    {
        boardScript = GetComponent<BoardManager>();
        hexHolderTransform = new GameObject("Overlay").transform; // child all "hexOverlay" under parent "Overlay"
        hexHolderTransform.position = boardScript.GetHexPosition(x, y);
        GameObject instance;

        // top
        instance = InstantiateOverlay(x, y + 1);
        if (instance)
        {
            instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
            hexScriptTop = instance.GetComponent<HexOverlay>();
        }

        // bottom
        instance = InstantiateOverlay(x, y - 1);
        if (instance)
        {
            instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
            hexScriptBottom = instance.GetComponent<HexOverlay>();
        }

        if (x % 2 == 0) // x is even
        {
            // top left
            instance = InstantiateOverlay(x - 1, y);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptTopLeft = instance.GetComponent<HexOverlay>();
            }

            // top right
            instance = InstantiateOverlay(x + 1, y);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptTopRight = instance.GetComponent<HexOverlay>();
            }

            // bottom left
            instance = InstantiateOverlay(x - 1, y - 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptBottomLeft = instance.GetComponent<HexOverlay>();
            }

            // bottom right
            instance = InstantiateOverlay(x + 1, y - 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptBottomRight = instance.GetComponent<HexOverlay>();
            }
        }
        else // x is odd
        {
            // top left
            instance = InstantiateOverlay(x - 1, y + 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptTopLeft = instance.GetComponent<HexOverlay>();
            }

            // top right
            instance = InstantiateOverlay(x + 1, y + 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptTopRight = instance.GetComponent<HexOverlay>();
            }

            // bottom left
            instance = InstantiateOverlay(x - 1, y);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptBottomLeft = instance.GetComponent<HexOverlay>();
            }

            // bottom right
            instance = InstantiateOverlay(x + 1, y);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptBottomRight = instance.GetComponent<HexOverlay>();
            }
        }

        // 
        updateHexInfo(x, y);
    }

    // Called after each movement to update the properties of each hex
    void updateHexInfo(int x, int y)
    {
        if (hexScriptTop)
        {
            hexScriptTop.OverlaySetActive(boardScript.isHexValid(x, y + 1));
            hexScriptTop.x = x;
            hexScriptTop.y = y + 1;
        }

        if (hexScriptBottom)
        {
            hexScriptBottom.OverlaySetActive(boardScript.isHexValid(x, y - 1));
            hexScriptBottom.x = x;
            hexScriptBottom.y = y - 1;
        }

        if (x % 2 == 0) // x is even
        {
            if (hexScriptTopLeft)
            {
                hexScriptTopLeft.OverlaySetActive(boardScript.isHexValid(x - 1, y));
                hexScriptTopLeft.x = x - 1;
                hexScriptTopLeft.y = y;
            }

            if (hexScriptTopRight)
            {
                hexScriptTopRight.OverlaySetActive(boardScript.isHexValid(x + 1, y));
                hexScriptTopRight.x = x + 1;
                hexScriptTopRight.y = y;
            }

            if (hexScriptBottomLeft)
            {
                hexScriptBottomLeft.OverlaySetActive(boardScript.isHexValid(x - 1, y - 1));
                hexScriptBottomLeft.x = x - 1;
                hexScriptBottomLeft.y = y - 1;
            }

            if (hexScriptBottomRight)
            {
                hexScriptBottomRight.OverlaySetActive(boardScript.isHexValid(x + 1, y - 1));
                hexScriptBottomRight.x = x + 1;
                hexScriptBottomRight.y = y - 1;
            }
        }
        else // x is odd
        {
            if (hexScriptTopLeft)
            {
                hexScriptTopLeft.OverlaySetActive(boardScript.isHexValid(x - 1, y + 1));
                hexScriptTopLeft.x = x - 1;
                hexScriptTopLeft.y = y + 1;
            }

            if (hexScriptTopRight)
            {
                hexScriptTopRight.OverlaySetActive(boardScript.isHexValid(x + 1, y + 1));
                hexScriptTopRight.x = x + 1;
                hexScriptTopRight.y = y + 1;
            }

            if (hexScriptBottomLeft)
            {
                hexScriptBottomLeft.OverlaySetActive(boardScript.isHexValid(x - 1, y));
                hexScriptBottomLeft.x = x - 1;
                hexScriptBottomLeft.y = y;
            }

            if (hexScriptBottomRight)
            {
                hexScriptBottomRight.OverlaySetActive(boardScript.isHexValid(x + 1, y));
                hexScriptBottomRight.x = x + 1;
                hexScriptBottomRight.y = y;
            }
        }
    }

    GameObject InstantiateOverlay(int x, int y)
    {
        // Check if x and y are out of bounds or a wall
        if (!boardScript.isHexValid(x, y))
        {
            return null;
        }

        // Create the overlay
        Vector3 overlayPosition = boardScript.GetHexPosition(x, y);
        GameObject instance = Instantiate(hexOverlay, overlayPosition, Quaternion.identity) as GameObject;
        instance.name = "hexOverlay";
        return instance;
    }

}
