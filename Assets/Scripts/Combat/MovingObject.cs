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

    // Remaining move range based on AP
    public int MoveRange { get { return CurrentAp / Constants.ApCostPerMove; } }

    // Duration of the movement animation in seconds
    public float MoveTime = 0.1f;

    // ======================================
    // Public Functions
    // ======================================

    // Adds a move action to the end of the queue
    public virtual void QueueMoveAction(int targetX, int targetY)
    {
        var path = CombatBoardManager.Instance.GetTilesInPath(X, Y, targetX, targetY);
        var moveAction = new MoveAction(targetX, targetY, path);
        ActionQueue.Add(moveAction);

        // Decrease currentAp
        CurrentAp -= path.Count * Constants.ApCostPerMove;
    }

    // Removes a queued action by index
    public void DequeueAction(int index)
    {
        CurrentAp -= ActionQueue[index].RequiredAp;
        ActionQueue.RemoveAt(index);

        // Update UI visuals for action removed
    }

    // Runs all the actions in the action queue
    public void RunCombatActions()
    {
        ActionsComplete = true;
        _actionIndex = 0;
        ProcessNextCombatAction();
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
        var action = ActionQueue[_actionIndex];
        _actionIndex++;
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
    }

    // Processes the movement action
    // callback indicates what function will be run when current execution completes
    public void RunMoveAction(MoveAction action, Action callback)
    {
        _inverseMoveTime = 1f / MoveTime;

        // Set the ending action
        _EndActionCallback = callback;

        // Update object coordinates
        X = action.TargetX;
        Y = action.TargetY;

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

    // Runs the ending action
    void EndCombatAction()
    {
        if (_EndActionCallback != null)
            _EndActionCallback();
    }

    // Updates the sprite's sorting layer so that they render in the right order
    void UpdateSortingLayer()
    {
        _sprite.sortingOrder = (int)((-1) * transform.position.y);
    }
}
