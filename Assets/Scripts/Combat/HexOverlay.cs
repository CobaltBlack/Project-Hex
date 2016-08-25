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
    SpriteRenderer _overlaySprite;
    
    public int X;
    public int Y;

    void Awake()
    {
        _overlaySprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        _overlaySprite.material.color = Color.green; // color wont work because most of the sprite is transparent
        _overlaySprite.enabled = false; // disable sprite for testing purposes
    }

    void OnMouseExit()
    {
        _overlaySprite.material.color = Color.white;
        _overlaySprite.enabled = true;
    }

    void OnMouseUp()
    {
        CombatManager.Instance.HandleHexClick(X, Y);
    }

    public void OverlaySetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}