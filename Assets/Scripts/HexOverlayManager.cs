using UnityEngine;
using System.Collections;

public class HexOverlayManager : MonoBehaviour
{

    public BoardManager boardScript;
    public GameManager gameScript;

    public GameObject hexOverlay;

    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        gameScript = GetComponent<GameManager>();
    }

    public void moveOverlay(int x, int y)
    {
        hexHolder.position = new Vector3(boardScript.gameBoard[x, y].x, boardScript.gameBoard[x, y].y, 0f);

        if (x % 2 == 1) // x is odd
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
        else // x is even
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
    }

    public void destroyOverlay()
    {
        //GameObject toDestroy = GameObject.Find("Overlay");
        Destroy(hexHolder.gameObject);
    }

    Transform hexHolder;

    HexOverlay hexScriptTop;
    HexOverlay hexScriptBottom;
    HexOverlay hexScriptTopLeft;
    HexOverlay hexScriptTopRight;
    HexOverlay hexScriptBottomLeft;
    HexOverlay hexScriptBottomRight;

    public void createOverlay(float x, float y) // take in TRUE coordinate
    {
        hexHolder = new GameObject("Overlay").transform; // child all "hexOverlay" under parent "Overlay"
        hexHolder.position = boardScript.trueCenter;
        GameObject instance;

        if (x % 2 == 1) // x is odd
        {
            // top
            instance = Instantiate(hexOverlay, new Vector3(x, y + 1, 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptTop = instance.GetComponent<HexOverlay>();
            hexScriptTop.xWorld = boardScript.xWorldCenter;
            hexScriptTop.yWorld = boardScript.yWorldCenter + 1;

            // bottom
            instance = Instantiate(hexOverlay, new Vector3(x, y - 1, 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptBottom = instance.GetComponent<HexOverlay>();
            hexScriptBottom.xWorld = boardScript.xWorldCenter;
            hexScriptBottom.yWorld = boardScript.yWorldCenter - 1;

            // top left
            instance = Instantiate(hexOverlay, new Vector3((float)(x - 1.5), (float)(y + 0.5), 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptTopLeft = instance.GetComponent<HexOverlay>();
            hexScriptTopLeft.xWorld = boardScript.xWorldCenter - 1;
            hexScriptTopLeft.yWorld = boardScript.yWorldCenter + 1;

            // top right
            instance = Instantiate(hexOverlay, new Vector3((float)(x + 1.5), (float)(y + 0.5), 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptTopRight = instance.GetComponent<HexOverlay>();
            hexScriptTopRight.xWorld = boardScript.xWorldCenter + 1;
            hexScriptTopRight.yWorld = boardScript.yWorldCenter + 1;

            // bottom left
            instance = Instantiate(hexOverlay, new Vector3((float)(x - 1.5), (float)(y - 0.5), 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptBottomLeft = instance.GetComponent<HexOverlay>();
            hexScriptBottomLeft.xWorld = boardScript.xWorldCenter - 1;
            hexScriptBottomLeft.yWorld = boardScript.yWorldCenter;

            // bottom right
            instance = Instantiate(hexOverlay, new Vector3((float)(x + 1.5), (float)(y - 0.5), 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptBottomRight = instance.GetComponent<HexOverlay>();
            hexScriptBottomRight.xWorld = boardScript.xWorldCenter + 1;
            hexScriptBottomRight.yWorld = boardScript.yWorldCenter;
        }
        else // x is even
        {
            // top
            instance = Instantiate(hexOverlay, new Vector3(x, y + 1, 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptTop = instance.GetComponent<HexOverlay>();
            hexScriptTop.xWorld = boardScript.xWorldCenter;
            hexScriptTop.yWorld = boardScript.yWorldCenter + 1;

            // bottom
            instance = Instantiate(hexOverlay, new Vector3(x, y - 1, 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptBottom = instance.GetComponent<HexOverlay>();
            hexScriptBottom.xWorld = boardScript.xWorldCenter;
            hexScriptBottom.yWorld = boardScript.yWorldCenter - 1;

            // top left
            instance = Instantiate(hexOverlay, new Vector3((float)(x - 1.5), (float)(y + 0.5), 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptTopLeft = instance.GetComponent<HexOverlay>();
            hexScriptTopLeft.xWorld = boardScript.xWorldCenter - 1;
            hexScriptTopLeft.yWorld = boardScript.yWorldCenter;

            // top right
            instance = Instantiate(hexOverlay, new Vector3((float)(x + 1.5), (float)(y + 0.5), 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptTopRight = instance.GetComponent<HexOverlay>();
            hexScriptTopRight.xWorld = boardScript.xWorldCenter + 1;
            hexScriptTopRight.yWorld = boardScript.yWorldCenter;

            // bottom left
            instance = Instantiate(hexOverlay, new Vector3((float)(x - 1.5), (float)(y - 0.5), 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptBottomLeft = instance.GetComponent<HexOverlay>();
            hexScriptBottomLeft.xWorld = boardScript.xWorldCenter - 1;
            hexScriptBottomLeft.yWorld = boardScript.yWorldCenter - 1;

            // bottom right
            instance = Instantiate(hexOverlay, new Vector3((float)(x + 1.5), (float)(y - 0.5), 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
            instance.name = "hexOverlay"; // name the hexes by coordinates

            hexScriptBottomRight = instance.GetComponent<HexOverlay>();
            hexScriptBottomRight.xWorld = boardScript.xWorldCenter + 1;
            hexScriptBottomRight.yWorld = boardScript.yWorldCenter - 1;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
