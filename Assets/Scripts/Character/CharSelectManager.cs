using UnityEngine;
using System.Collections;

public class CharSelectManager : MonoBehaviour
{
    private Character characterData;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void LoadGame(Character chosenCharacterData)
    {
        characterData = chosenCharacterData;

        Application.LoadLevel("Map");
    }
}