using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    protected Pathfinder pathfinder;
    [SerializeField]
    protected float speed = 5f;
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    protected Vector3 direction;

    private List<Vector3> path = new List<Vector3>();
    private int currentNodeIndex;
    private Vector3 lastGoalPosition;

    private void OnDrawGizmos()
    {
        DrawPath();
    }

    protected void MoveTo(Transform destination)
    {
        Vector3 goalPosition = destination.position;

        MoveTo(goalPosition);
    }

    protected void MoveTo(Vector3 goalPosition)
    {
        bool hasGoalChanged = goalPosition != lastGoalPosition;

        if (currentNodeIndex == path.Count || hasGoalChanged)
        {
            path.Clear();
            path = pathfinder.RequestPath(transform.position, goalPosition);
            currentNodeIndex = 0;
        }

        bool isPathEmpty = path.Count == 0;

        if (isPathEmpty)
        {
            return;
        }

        Vector3 currentNode = path[currentNodeIndex];

        direction = currentNode - transform.position;

        transform.Translate(speed * Time.deltaTime * direction.normalized);

        float distanceToNextNode = Vector3.Distance(transform.position, currentNode);

        if (distanceToNextNode < 0.1f)
        {
            currentNodeIndex++;
        }

        lastGoalPosition = goalPosition;
    }

    protected bool HasReachedDestination(Transform destination)
    {
        float distanceToDestination = Vector3.Distance(transform.position, destination.position);

        return distanceToDestination < 0.5f;
    }

    protected void DoMoveAnimation(string character, string action)
    {
        int horizontal = (int)direction.x;
        int vertical = (int)direction.y;

        if (horizontal > 0)
        {
            animator.Play($"Base Layer.{character}_{action}_Side");
            spriteRenderer.flipX = true;
        }
        else if (horizontal < 0)
        {
            animator.Play($"Base Layer.{character}_{action}_Side");
            spriteRenderer.flipX = false;
        }

        else if (vertical < 0)
        {
            animator.Play($"Base Layer.{character}_{action}_Front");
        }
        else if (vertical > 0)
        {
            animator.Play($"Base Layer.{character}_{action}_Back");
        }
    }

    private void DrawPath()
    {
        Gizmos.color = Color.blue;

        for (int i = 0; i < path.Count; i++)
        {
            if (i + 1 >= path.Count)
            {
                continue;
            }

            Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }
}
