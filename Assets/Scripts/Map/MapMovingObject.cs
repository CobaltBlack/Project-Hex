using UnityEngine;
using System.Collections;

public class MapMovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    private float inverseMoveTime;

    private bool isMoving;

    // Use this for initialization
    void Start ()
    {
        inverseMoveTime = 1f / moveTime; // inverse lets us multiply instead of divide for efficiency
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void MoveObject(Transform MovingObject, Vector3 end)
    {
        Rigidbody2D rb2D = MovingObject.GetComponent<Rigidbody2D>();

        StartCoroutine(SmoothMovement(MovingObject, rb2D, end));
    }

    private IEnumerator SmoothMovement(Transform MovingObject, Rigidbody2D rb2D, Vector3 end) // COROUTINE(function executed in intervals)! end = where to move to
    {
        Debug.Log("Start Smooth Movement");
        isMoving = true;

        float sqrRemainingDistance = (MovingObject.position - end).sqrMagnitude; // calculate the remaining distance to move

        //while (sqrRemainingDistance > float.Epsilon) // float.Epsilon = almost 0
        while (sqrRemainingDistance > 0.005f) // almost zero
        {
            Vector3 newPosition = Vector3.Lerp(rb2D.position, end, inverseMoveTime * Time.deltaTime); // find newPosition proportionally closer to the end based on movetime
            rb2D.MovePosition(newPosition); // move rigid body
            sqrRemainingDistance = (MovingObject.position - end).sqrMagnitude; // recalculate remaining distance
            yield return null; // wait for a frame before re evaluating the condition of the loop
        }

        Debug.Log("End Smooth Movement");
        isMoving = false;
    }
}
