using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

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
}
