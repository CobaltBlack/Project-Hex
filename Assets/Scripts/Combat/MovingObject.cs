using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MovingObject : MonoBehaviour
{
    public string Name;

    public int X;
    public int Y;

    public int CurrentHp;
    public int MaxHp;

    public int CurrentAp;
    public int MaxAp;

    public bool ActionsComplete = false;

    public List<CombatAction> ActionQueue = new List<CombatAction>();

    // TODO: Calculate range based on remaining AP
    public int MoveRange { get { return 2; } }

    public float MoveTime = 0.1f;

    // ======================================
    // Public Functions
    // ======================================

    // Adds a move action to the end of the queue
    public virtual void QueueMoveAction(int targetX, int targetY)
    {
        var path = CombatBoardManager.Instance.GetTilesInPath(X, Y, targetX, targetY);
        MoveAction moveAction = new MoveAction(targetX, targetY, path);
        ActionQueue.Add(moveAction);

        // Decrease currentAp

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
        _ActionIndex = 0;
        ProcessNextCombatAction();
    }

    // Removes all actions currently in the queue
    public void DequeueAllActions()
    {
        ActionQueue.Clear();

        // Update UI visuals
    }

    // ======================================
    // Private Functions
    // ======================================

    int _ActionIndex = 0;
    float _InverseMoveTime;
    List<HexTile> _CurrentPath;
    int _CurrentPathIndex;

    Action _EndActionCallback = null;

    // Process the next combat action in the ActionQueue
    void ProcessNextCombatAction()
    {
        // If queue is done, run the queue of the next character
        if (_ActionIndex >= ActionQueue.Count)
        {
            CombatManager.Instance.ProcessNextCharacterActions();
            return;
        }

        // Run action based on its type
        var action = ActionQueue[_ActionIndex];
        switch (action.ActionType)
        {
            case ActionType.Move:
                RunMoveAction((MoveAction)action, ProcessNextCombatAction);
                break;

            case ActionType.Skill:
                break;

            case ActionType.Item:
                break;

            default:
                break;
        }

        _ActionIndex++;
    }

    // Processes the movement action
    // callback indicates what function will be run when current execution completes
    public void RunMoveAction(MoveAction action, Action callback)
    {
        _InverseMoveTime = 1f / MoveTime;

        // Update object coordinates
        X = action.TargetX;
        Y = action.TargetY;

        // Set the ending action
        _EndActionCallback = callback;

        // Move to each tile in the path
        _CurrentPath = action.Path;
        _CurrentPathIndex = 0;
        StartNextSmoothMove();
    }

    // Move to the next tile in the path using a SmoothMove
    // This function is called recursively until the path is complete
    void StartNextSmoothMove()
    {
        // If we are done, run the next action
        if (_CurrentPathIndex >= _CurrentPath.Count)
        {
            EndCombatAction();
            return;
        }

        // Otherwise, do a SmoothMove
        var endPosition = _CurrentPath[_CurrentPathIndex].Position;
        _CurrentPathIndex++;
        StartCoroutine(SmoothMove(endPosition));
    }

    // This coroutine runs the function callback() when it is complete
    IEnumerator SmoothMove(Vector3 end)
    {
        float sqrRemainingDistance = (gameObject.transform.position - end).sqrMagnitude;
        
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(gameObject.transform.position, end, _InverseMoveTime * Time.deltaTime);
            gameObject.transform.position = newPosition;
            sqrRemainingDistance = (gameObject.transform.position - end).sqrMagnitude;
            yield return null;
        }

        StartNextSmoothMove();
    }

    // Runs the ending action
    void EndCombatAction()
    {
        if (_EndActionCallback != null)
            _EndActionCallback();
    }
}
