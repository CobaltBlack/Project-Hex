using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingObject : MonoBehaviour
{
    public string Name;

    public int PositionX;
    public int PositionY;

    public int CurrentHp;
    public int MaxHp;

    public int CurrentAp;
    public int MaxAp;
    
    public bool ActionsComplete = false;
    public GameObject CharacterShadow = null;

    public List<CombatAction> ActionQueue = new List<CombatAction>();

    // Adds a move action to the end of the queue
    public void QueueMoveAction(int x, int y)
    {
        MoveAction moveAction = new MoveAction(x, y);
        ActionQueue.Add(moveAction);

        // Update UI visuals for new action queued
    }

    // Removes a queued action by index
    public void DequeueAction(int index)
    {
        ActionQueue.RemoveAt(index);

        // Update UI visuals for action removed
    }

    // Runs all the actions in the action queue
    public void RunCombatActions()
    {
        ActionsComplete = true;
        if (ActionQueue.Count == 0)
        {
            CombatManager.Instance.ProcessNextCharacterActions();
        }
        else
        {
            StartCoroutine(RunCombatActionsCoroutine());
        }
    }

    // Removes all actions currently in the queue
    public void DequeueAllActions()
    {
        ActionQueue.Clear();

        // Update UI visuals
    }

    // Move object to position with animation
    public void MoveToPosition(int x, int y)
    {
        PositionX = x;
        PositionY = y;
        gameObject.transform.position = CombatBoardManager.Instance.GetHexPosition(x, y);

        // TODO: Run Dijkstra's or some pathfindng algorithm

        // TODO: Smoothly animate moving smoothly

    }

    // Using coroutine allows the program to Wait until animations are complete
    IEnumerator RunCombatActionsCoroutine()
    {

        foreach (var action in ActionQueue)
        {
            // Update UI visuals for executing action

            // Run actions
            switch (action.ActionType)
            {
                case ActionType.Move:
                    RunMoveAction((MoveAction)action);
                    break;

                case ActionType.Skill:
                    break;

                case ActionType.Item:
                    break;

                default:
                    break;
            }

            yield return new WaitForSeconds(1);
        }
        Debug.Log("DONE ACTIONS");

        // Process actions of next character, if there are any
        // Otherwise, this starts the enemy turn
        CombatManager.Instance.ProcessNextCharacterActions();
    }

    void RunMoveAction(MoveAction action)
    {
        MoveToPosition(action.TargetX, action.TargetY);
    }
}
