using UnityEngine;
using System.Collections;
using System;

public class ProjectileEffect : MonoBehaviour {

    public float MoveTime;

    float _inverseMoveTime;
    Action _callback;

    // Use this for initialization
    void Start () {
        _inverseMoveTime = 1f / MoveTime;
    }

    // Moves the projectile towards targetObj.
    // Executes callback() once animation is complete
    public void MoveTowards(MovingObject targetObj, Action callback)
    {
        _callback = callback;
        var endPosition = targetObj.transform.position;
        StartCoroutine(SmoothMove(endPosition));
    }

    IEnumerator SmoothMove(Vector3 end)
    {
        float sqrRemainingDistance = (gameObject.transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            // Get next position to move based on time passed
            var newPosition = Vector3.MoveTowards(gameObject.transform.position, end, _inverseMoveTime * Time.deltaTime);
            gameObject.transform.position = newPosition;
            sqrRemainingDistance = (gameObject.transform.position - end).sqrMagnitude;
            yield return null;
        }

        _callback();
        Destroy(gameObject);
    }
}
