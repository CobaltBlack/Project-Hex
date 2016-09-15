using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/*
 * InstanceManager
 * 
 * This script manages the starting of instances
 * 
 */

public class InstanceManager : MonoBehaviour
{
    private ModalPanelInstance modalPanel;
    private DisplayManager displayManager; // this manager is currently used to display visual debug messages, but may later be repurposed for in game announcements.

    void Awake()
    {
        modalPanel = ModalPanelInstance.Instance;
        displayManager = DisplayManager.Instance;
    }
    
    // The test button calls this function:
    public void TestInstance()
    {
        // ExampleInstance is a child class of Instance
        StartInstance(new ExampleInstance());
    }

    // TODO: Choose random instance to start from list of all instances
    // Maybe set types for each instance, and then adjust the chance each type can appear
    public void StartRandomInstance()
    {

    }

    public void StartInstance(Instance instance)
    {
        displayManager = DisplayManager.Instance;
        displayManager.DisplayMessage("Starting instance.");

        modalPanel.DisplayPrompt(instance.InitialPrompt);
    }

    public List<GameObject> instanceGameObjectList;

    public Instance AssignInstance(Tileset tilesetTag, Layout layoutTag)
    {
        List<Instance> validInstanceList = new List<Instance>();

        for (int i = 0; i < instanceGameObjectList.Count; i++)
        {
            Instance currentInstance = instanceGameObjectList[i].GetComponent<Instance>();

            for (int u = 0; u < currentInstance.InstanceTileset.Count; u++)
            {
                for (int o = 0; o < currentInstance.InstanceTileset.Count; o++)
                {
                    if (currentInstance.InstanceTileset[u] == tilesetTag && currentInstance.InstanceLayout[o] == layoutTag)
                    {
                        validInstanceList.Add(currentInstance);
                    }
                }
            }
        }

        // PLACE HOLDER CODE - write a proper exception case for when there is no matching instance//
        if (validInstanceList.Count == 0)
        {
            return instanceGameObjectList[0].GetComponent<Instance>();
        }
        // PLACE HOLDER CODE - write a proper exception case for when there is no matching instance //

        return validInstanceList[Random.Range(0, validInstanceList.Count)];
    }
}
