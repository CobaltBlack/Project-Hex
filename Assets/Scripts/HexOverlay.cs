using UnityEngine;
using System.Collections;

/*
 * HexOverlay
 * 
 * This script is attached to individual overlay objects and detects input from the player
 *  
 */

public class HexOverlay : MonoBehaviour
{
    SpriteRenderer overlaySprite;

    // WORLD coordinate
    public int xWorld;
    public int yWorld;

    void Awake()
    {
        overlaySprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        overlaySprite.material.color = Color.green; // color wont work because most of the sprite is transparent
        overlaySprite.enabled = false; // disable sprite for testing purposes
    }

    void OnMouseExit()
    {
        overlaySprite.material.color = Color.white;
        overlaySprite.enabled = true;
    }

    void OnMouseUp()
    {
        GameManager.instance.handleHexClick(xWorld, yWorld);
    }

    public void OverlaySetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}