﻿using UnityEngine;
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

    public List<SkillData> Skills = new List<SkillData>();

    public bool ActionsComplete = false;

    // Remaining move range based on AP
    public int MoveRange { get { return CurrentAp / Constants.ApCostPerMove; } }

    // ======================================
    // Public Functions
    // ======================================

    // Adds a move action to the end of the queue
    public virtual void QueueMoveAction(int targetX, int targetY)
    {
        var path = CombatBoardManager.Instance.GetTilesInPath(X, Y, targetX, targetY, false);
        var moveAction = new MoveAction(path);
        ActionQueue.Add(moveAction);

        // Decrease currentAp
        CurrentAp -= path.Count * Constants.ApCostPerMove;

        // Set the object queued on target tile, so other objects can't move on it
        CombatBoardManager.Instance.SetObjectOnTileQueued(targetX, targetY, this);
    }

    // Adds an action for an InstantSkill to the ActionQueue
    public void QueueInstantSkill(InstantSkill skill)
    {
        CurrentAp -= skill.RequiredAp;
        var action = new InstantSkillAction(skill);
        action.SourceObject = this;
        ActionQueue.Add(action);
    }

    // Adds an action for an RangedSkill to the ActionQueue
    public void QueueRangedSkill(RangedSkill skill, int targetX, int targetY)
    {

    }

    // Runs all the actions in the action queue
    public void RunCombatActions()
    {
        ActionsComplete = true;
        _actionIndex = 0;
        ProcessNextCombatAction();
    }

    // Removes a queued action by index
    public void DequeueAction(int index)
    {
        CurrentAp += ActionQueue[index].RequiredAp;
        ActionQueue.RemoveAt(index);

        // Update UI visuals for action removed
    }

    // Removes all actions currently in the queue
    public void DequeueAllActions()
    {
        ActionQueue.Clear();
        CurrentAp = MaxAp;

        // Update UI visuals
    }

    // ======================================
    // Private Functions
    // ======================================

    protected List<CombatAction> ActionQueue = new List<CombatAction>();

    // Duration of the movement animation in seconds
    float MoveTime = 0.1f;

    int _actionIndex = 0;
    float _inverseMoveTime;
    List<HexTile> _currentPath;
    int _currentPathIndex;

    Action _EndActionCallback = null;

    SpriteRenderer _sprite;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        UpdateSortingLayer();
    }

    // Process the next combat action in the ActionQueue
    void ProcessNextCombatAction()
    {
        // If queue is done, run the queue of the next character
        if (_actionIndex >= ActionQueue.Count)
        {
            CombatManager.Instance.ProcessNextCharacterActions();
            return;
        }

        // Run action based on its type
        // The functions will call ProcessNextCombatAction() once it completes
        var action = ActionQueue[_actionIndex];
        _actionIndex++;
        switch (action.ActionType)
        {
            case ActionType.Move:
                RunMoveAction((MoveAction)action, ProcessNextCombatAction, true);
                break;

            case ActionType.Skill:
                RunSkillAction((SkillAction)action);
                break;

            case ActionType.Item:
                break;

            default:
                break;
        }
    }

    // Processes the movement action
    // - callback indicates what function will be run when current execution completes
    // - updateTiles indicates whether tiles should update to store the current object
    public void RunMoveAction(MoveAction action, Action callback, bool updateTiles)
    {
        _inverseMoveTime = 1f / MoveTime;

        // Set the ending action
        _EndActionCallback = callback;

        // Update object coordinates
        X = action.TargetX;
        Y = action.TargetY;

        if (updateTiles)
        {
            CombatBoardManager.Instance.SetObjectOnTile(X, Y, this);
        }

        // Move to each tile in the path
        _currentPath = action.Path;
        _currentPathIndex = 0;
        StartNextSmoothMove();
    }

    // Move to the next tile in the path using a SmoothMove
    // This function is called recursively until the path is complete
    void StartNextSmoothMove()
    {
        // If we are done, run the next action
        if (_currentPathIndex >= _currentPath.Count)
        {
            EndCombatAction();
            return;
        }

        // Otherwise, do a SmoothMove
        var nextTile = _currentPath[_currentPathIndex];
        _currentPathIndex++;

        var endPosition = nextTile.Position;
        StartCoroutine(SmoothMove(endPosition));
    }

    // This coroutine runs the function callback() when it is complete
    IEnumerator SmoothMove(Vector3 end)
    {
        float sqrRemainingDistance = (gameObject.transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            // Get next position to move based on time passed
            var newPosition = Vector3.MoveTowards(gameObject.transform.position, end, _inverseMoveTime * Time.deltaTime);
            gameObject.transform.position = newPosition;
            sqrRemainingDistance = (gameObject.transform.position - end).sqrMagnitude;
            UpdateSortingLayer();

            yield return null;
        }

        StartNextSmoothMove();
    }

    // Updates the sprite's sorting layer so that they render in the right order
    void UpdateSortingLayer()
    {
        _sprite.sortingOrder = (int)((-1) * transform.position.y);
    }

    // Makes the current character run the SkillAction
    // Calls ProcessNextCombatAction() once it is complete
    public void RunSkillAction(SkillAction action)
    {
        action.ExecuteSkill(ProcessNextCombatAction);
    }

    // Runs the ending action
    void EndCombatAction()
    {
        if (_EndActionCallback != null)
            _EndActionCallback();
    }
}
