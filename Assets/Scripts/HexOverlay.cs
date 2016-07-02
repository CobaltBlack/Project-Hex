using UnityEngine;
using System.Collections;

public class HexOverlay : MonoBehaviour
{
    SpriteRenderer overlaySprite;

    // WORLD coordinate
    public int xWorld;
    public int yWorld;

    // TRUE coordinate
    //public float xTrue;
    //public float yTrue;

    public void Awake()
    {
        overlaySprite = gameObject.GetComponent<SpriteRenderer>();

        //xTrue = gameObject.transform.position.x;
        //yTrue = gameObject.transform.position.y;
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
        GameManager.instance.inputSequence(xWorld, yWorld);
    }

}