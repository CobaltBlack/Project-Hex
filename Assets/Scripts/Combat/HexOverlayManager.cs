using UnityEngine;
using System.Collections;

/*
 * HexOverlayManager
 * 
 * This script controls the clickable overlays that receives input from the player
 */

public class HexOverlayManager : MonoBehaviour
{
    public GameObject HexOverlayObject;

    GameObject OverlayContainer;

    // Instantiates a clickable overlay for each tile given
    public void InstantiateOverlays(HexTile[] tiles)
    {
        OverlayContainer = new GameObject("HexOverlays");
        for (int i = 0; i < tiles.Length; i++)
        {
            GameObject overlayObj = InstantiateOverlay(tiles[i]);
            overlayObj.transform.SetParent(OverlayContainer.transform);
        }
    }

    public void RemoveAllOverlays()
    {
        Destroy(OverlayContainer);
    }

    // Instantiates and returns an overlay for the given HexTile
    GameObject InstantiateOverlay(HexTile tile)
    {
        // Instantiate gameObject
        GameObject instance = Instantiate(HexOverlayObject, tile.Position, Quaternion.identity) as GameObject;

        // Set some attributes
        instance.name = "hexOverlay";
        HexOverlay overlayScript = instance.GetComponent<HexOverlay>();
        overlayScript.X = tile.X;
        overlayScript.Y = tile.Y;
        return instance;
    }
}
