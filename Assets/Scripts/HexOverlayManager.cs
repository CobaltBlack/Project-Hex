using UnityEngine;
using System.Collections;

public class HexOverlayManager : MonoBehaviour
{
    public GameObject hexOverlay;

    BoardManager boardScript;
    GameManager gameScript;

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
        gameScript = GetComponent<GameManager>();
    }

    public void moveOverlay(int x, int y)
    {
        hexHolderTransform.position = boardScript.GetHexPosition(x, y);

        if (x % 2 == 0) // x is even
        {
            hexScriptTop.xWorld = x;
            hexScriptTop.yWorld = y + 1;

            hexScriptBottom.xWorld = x;
            hexScriptBottom.yWorld = y - 1;

            hexScriptTopLeft.xWorld = x - 1;
            hexScriptTopLeft.yWorld = y;

            hexScriptTopRight.xWorld = x + 1;
            hexScriptTopRight.yWorld = y;

            hexScriptBottomLeft.xWorld = x - 1;
            hexScriptBottomLeft.yWorld = y - 1;

            hexScriptBottomRight.xWorld = x + 1;
            hexScriptBottomRight.yWorld = y - 1;
        }
        else // x is odd
        {
            hexScriptTop.xWorld = x;
            hexScriptTop.yWorld = y + 1;

            hexScriptBottom.xWorld = x;
            hexScriptBottom.yWorld = y - 1;

            hexScriptTopLeft.xWorld = x - 1;
            hexScriptTopLeft.yWorld = y + 1;

            hexScriptTopRight.xWorld = x + 1;
            hexScriptTopRight.yWorld = y + 1;

            hexScriptBottomLeft.xWorld = x - 1;
            hexScriptBottomLeft.yWorld = y;

            hexScriptBottomRight.xWorld = x + 1;
            hexScriptBottomRight.yWorld = y;
        }
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

        if (x % 2 == 0) // x is even
        {
            // top
            instance = InstantiateOverlay(x, y + 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptTop = instance.GetComponent<HexOverlay>();
                hexScriptTop.xWorld = x;
                hexScriptTop.yWorld = y + 1;
            }

            // bottom
            instance = InstantiateOverlay(x, y - 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptBottom = instance.GetComponent<HexOverlay>();
                hexScriptBottom.xWorld = x;
                hexScriptBottom.yWorld = y - 1;
            }

            // top left
            instance = InstantiateOverlay(x - 1, y);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptTopLeft = instance.GetComponent<HexOverlay>();
                hexScriptTopLeft.xWorld = x - 1;
                hexScriptTopLeft.yWorld = y;
            }

            // top right
            instance = InstantiateOverlay(x + 1, y);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptTopRight = instance.GetComponent<HexOverlay>();
                hexScriptTopRight.xWorld = x + 1;
                hexScriptTopRight.yWorld = y;
            }

            // bottom left
            instance = InstantiateOverlay(x - 1, y - 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptBottomLeft = instance.GetComponent<HexOverlay>();
                hexScriptBottomLeft.xWorld = x - 1;
                hexScriptBottomLeft.yWorld = y - 1;
            }

            // bottom right
            instance = InstantiateOverlay(x + 1, y - 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptBottomRight = instance.GetComponent<HexOverlay>();
                hexScriptBottomRight.xWorld = x + 1;
                hexScriptBottomRight.yWorld = y - 1;
            }
        }
        else // x is odd
        {
            // top
            instance = InstantiateOverlay(x, y + 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptTop = instance.GetComponent<HexOverlay>();
                hexScriptTop.xWorld = x;
                hexScriptTop.yWorld = y + 1;
            }

            // bottom
            instance = InstantiateOverlay(x, y - 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptBottom = instance.GetComponent<HexOverlay>();
                hexScriptBottom.xWorld = x;
                hexScriptBottom.yWorld = y - 1;
            }

            // top left
            instance = InstantiateOverlay(x - 1, y + 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptTopLeft = instance.GetComponent<HexOverlay>();
                hexScriptTopLeft.xWorld = x - 1;
                hexScriptTopLeft.yWorld = y + 1;
            }

            // top right
            instance = InstantiateOverlay(x + 1, y + 1);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptTopRight = instance.GetComponent<HexOverlay>();
                hexScriptTopRight.xWorld = x + 1;
                hexScriptTopRight.yWorld = y + 1;
            }

            // bottom left
            instance = InstantiateOverlay(x - 1, y);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptBottomLeft = instance.GetComponent<HexOverlay>();
                hexScriptBottomLeft.xWorld = x - 1;
                hexScriptBottomLeft.yWorld = y;
            }

            // bottom right
            instance = InstantiateOverlay(x + 1, y);
            if (instance)
            {
                instance.transform.SetParent(hexHolderTransform);  // parent under hexHolder "Map"
                hexScriptBottomRight = instance.GetComponent<HexOverlay>();
                hexScriptBottomRight.xWorld = x + 1;
                hexScriptBottomRight.yWorld = y;
            }
        }
    }

    GameObject InstantiateOverlay(int x, int y)
    {
        // Check if x and y are out of bounds
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
