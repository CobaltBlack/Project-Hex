using UnityEngine;
using System.Collections;

/*
 * HexOverlayManager
 * 
 * This script controls the clickable overlays that receives input from the player
 */

public class HexOverlayManager : MonoBehaviour
{
    public GameObject hexOverlay;

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
}
