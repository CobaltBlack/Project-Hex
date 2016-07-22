using UnityEngine;
using System.Collections;

public class MapTile : MonoBehaviour
{
    SpriteRenderer overlaySprite;

    // tile size
    public int size = 0; // 0 = small, 1 = big !!!

    // EXIT information
    public GameObject exit1A;
    public GameObject exit1B;

    public GameObject exit2A;
    public GameObject exit2B;

    public GameObject exit3A;
    public GameObject exit3B;

    public GameObject exit4A;
    public GameObject exit4B;

    // touching tiles
    public GameObject[] nodes;

    // WORLD coordinate location
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
