﻿using UnityEngine;
using System.Collections;

/*
 * HexOverlayManager
 * 
 * This script controls the clickable overlays that receives input from the player
 *  
 */

public class HexOverlayManager : MonoBehaviour
{
    public GameObject hexOverlay;

    BoardManager boardScript;

    Transform hexHolderTransform;
    HexOverlay hexScriptTop;
    HexOverlay hexScriptBottom;
    HexOverlay hexScriptTopLeft;
    HexOverlay hexScriptTopRight;
    HexOverlay hexScriptBottomLeft;
    HexOverlay hexScriptBottomRight;

    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
    }

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

        updateHexInfo(x, y);
    }

    void updateHexInfo(int x, int y)
    {
        if (hexScriptTop)
        {
            hexScriptTop.OverlaySetActive(boardScript.isHexValid(x, y + 1));
            hexScriptTop.xWorld = x;
            hexScriptTop.yWorld = y + 1;
        }

        if (hexScriptBottom)
        {
            hexScriptBottom.OverlaySetActive(boardScript.isHexValid(x, y - 1));
            hexScriptBottom.xWorld = x;
            hexScriptBottom.yWorld = y - 1;
        }

        if (x % 2 == 0) // x is even
        {
            if (hexScriptTopLeft)
            {
                hexScriptTopLeft.OverlaySetActive(boardScript.isHexValid(x - 1, y));
                hexScriptTopLeft.xWorld = x - 1;
                hexScriptTopLeft.yWorld = y;
            }

            if (hexScriptTopRight)
            {
                hexScriptTopRight.OverlaySetActive(boardScript.isHexValid(x + 1, y));
                hexScriptTopRight.xWorld = x + 1;
                hexScriptTopRight.yWorld = y;
            }

            if (hexScriptBottomLeft)
            {
                hexScriptBottomLeft.OverlaySetActive(boardScript.isHexValid(x - 1, y - 1));
                hexScriptBottomLeft.xWorld = x - 1;
                hexScriptBottomLeft.yWorld = y - 1;
            }

            if (hexScriptBottomRight)
            {
                hexScriptBottomRight.OverlaySetActive(boardScript.isHexValid(x + 1, y - 1));
                hexScriptBottomRight.xWorld = x + 1;
                hexScriptBottomRight.yWorld = y - 1;
            }
        }
        else // x is odd
        {
            if (hexScriptTopLeft)
            {
                hexScriptTopLeft.OverlaySetActive(boardScript.isHexValid(x - 1, y + 1));
                hexScriptTopLeft.xWorld = x - 1;
                hexScriptTopLeft.yWorld = y + 1;
            }

            if (hexScriptTopRight)
            {
                hexScriptTopRight.OverlaySetActive(boardScript.isHexValid(x + 1, y + 1));
                hexScriptTopRight.xWorld = x + 1;
                hexScriptTopRight.yWorld = y + 1;
            }

            if (hexScriptBottomLeft)
            {
                hexScriptBottomLeft.OverlaySetActive(boardScript.isHexValid(x - 1, y));
                hexScriptBottomLeft.xWorld = x - 1;
                hexScriptBottomLeft.yWorld = y;
            }

            if (hexScriptBottomRight)
            {
                hexScriptBottomRight.OverlaySetActive(boardScript.isHexValid(x, y - 1));
                hexScriptBottomRight.xWorld = x + 1;
                hexScriptBottomRight.yWorld = y;
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
        instance.name = "hexOverlay"; // name the hexes by coordinates
        return instance;
    }
}
