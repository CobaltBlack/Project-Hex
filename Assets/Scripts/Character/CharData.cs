using UnityEngine;
using System.Collections;

public class CharData : MonoBehaviour
{
    // assign in inspector
    public Character.CharacterClass characterClass;
    public GameObject spotlight;

    private Character characterData;

    CharSelectManager charSelectManagerScript;

    void Awake()
    {
        charSelectManagerScript = GameObject.Find("CharSelectManager").GetComponent<CharSelectManager>();
        characterData = new Character(characterClass);
    }
    
    void OnMouseEnter()
    {
        spotlight.transform.position = gameObject.transform.position;
        spotlight.SetActive(true);
    }

    void OnMouseExit()
    {
        spotlight.SetActive(false);
    }

    void OnMouseUp()
    {
        charSelectManagerScript.LoadGame(characterData);
    }
}
