using UnityEngine;
using System; // enables [serializable] attribute... allows us to modify how variables appear in the inspector in the editor
using System.Collections.Generic; // Adding Generic enables lists
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

/*
 * GameManager
 * 
 * This script controls the general flow of the game 
 * and also initializes other scripts to do everything else.
 * 
 */

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    InventoryManager inventoryManager;
    PlayerManager playerManager;
    
    public CombatParameters combatParameters;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // gameObject refers to game object this component is attached to // NOTE IS THERE A BUG? NO RETURN?
        }

        DontDestroyOnLoad(gameObject); // To preserve game data such as score between stages
        
        inventoryManager = GetComponent<InventoryManager>();
        playerManager = GetComponent<PlayerManager>();

        InitializeGame();
    }

    public void InitializeGame()
    {
        // Initialize inventory
        Debug.Log("Initialize inventory");
        inventoryManager.InventorySetup();

        // Initialize player data
        Debug.Log("Initialize player data");
        playerManager.SetupPlayer();

        Debug.Log("Initialize game complete!");
    }

    public void StartCombat()
    {
        // Setup parameters

        // Load combat scene
        SceneManager.LoadScene("Combat");
    }
}
