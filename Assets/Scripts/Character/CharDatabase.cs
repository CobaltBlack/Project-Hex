using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

public class CharDatabase : MonoBehaviour
{
    private List<Character> database = new List<Character>();

    private JsonData characterData;

    void Awake()
    {
        characterData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Characters.json"));
        ConstructItemDatabase();
    }

    void ConstructItemDatabase()
    {

    }
}
