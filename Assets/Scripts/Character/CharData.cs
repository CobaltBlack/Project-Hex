using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

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
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            spotlight.transform.position = gameObject.transform.position;
            spotlight.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            spotlight.SetActive(false);
        }
    }

    void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            charSelectManagerScript.LoadGame(characterData);
        }
    }

    // to avoid spotlight staying on when esc is pressed
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            spotlight.SetActive(false);
        }
    }
}
