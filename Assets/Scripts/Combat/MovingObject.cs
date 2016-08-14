﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingObject : MonoBehaviour
{
    public int positionX;
    public int positionY;

    public int currentHp;
    public int maxHp;

    public int currentAp;
    public int maxAp;

    public bool isFriendly = false;
    public bool actionsComplete = false;


    public List<CombatAction> actionQueue = new List<CombatAction>();

    // Adds a move action to the end of the queue
    public void queueMoveAction(int x, int y)
    {
        MoveAction moveAction = new MoveAction(x, y);
        actionQueue.Add(moveAction);

        // If friendly character, animate movements for character's shadow

        // Update UI visuals for new action queued
    }

    // Removes a queued action by index
    public void dequeueAction(int index)
    {
        actionQueue.RemoveAt(index);

        // Update UI visuals for action removed
    }

    // Runs all the actions in the action queue
    public void runCombatActions()
    {
        StartCoroutine(runCombatActionsCoroutine());
        actionsComplete = true;
    }

    // Removes all actions currently in the queue
    public void dequeueAllActions()
    {
        actionQueue.Clear();

        // Update UI visuals
    }

    // Using coroutine allows the program to Wait until animations are complete
    IEnumerator runCombatActionsCoroutine()
    {
        foreach (var action in actionQueue)
        {
            // Update UI visuals for executing action

            // Run actions
            switch (action.actionType)
            {
                case ActionType.MOVE:
                    runMoveAction((MoveAction)action);
                    break;

                case ActionType.SKILL:
                    break;

                case ActionType.ITEM:
                    break;

                default:
                    break;
            }

            yield return new WaitForSeconds(1);
        }
        Debug.Log("DONE ACTIONS");

        // Process actions of next character, if there are any
        // Otherwise, this starts the enemy turn
        CombatManager.instance.ProcessNextCharacterActions();
    }

    void runMoveAction(MoveAction action)
    {
        moveToPosition(action.targetX, action.targetY);
    }

    // Move object to position (still used??)
    void moveToPosition(int x, int y)
    {
        positionX = x;
        positionY = y;
        gameObject.transform.position = CombatBoardManager.instance.GetHexPosition(x, y);

        // TODO: Run Dijkstra's or osme pathfindng algorithm

        // TODO: Smoothly animate moving smoothly

    }
}
