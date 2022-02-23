using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    protected Pathfinder pathfinder;
    [SerializeField]
    protected float speed = 5f;

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

        Vector3 direction = currentNode - transform.position;

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
