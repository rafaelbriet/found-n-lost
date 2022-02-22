using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostState { Idle, Searching, Chasing, Attacking }

public class Ghost : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float searchRadius = 2f;

    private Transform target;
    private Transform searchDestination;
    private GhostState currentState;

    private void Awake()
    {
        searchDestination = new GameObject().transform;
    }

    private void Update()
    {
        switch (currentState)
        {
            case GhostState.Idle:

                if (target == null)
                {
                    ChangeState(GhostState.Searching);
                }

                break;
            case GhostState.Searching:

                Transform targetFound = GetTarget();

                if (targetFound != null)
                {
                    target = targetFound;
                    ChangeState(GhostState.Chasing);
                }

                if (searchDestination == null || HasReachedDestination(searchDestination))
                {
                    GetSearchDestination();
                }

                MoveTo(searchDestination);

                break;
            case GhostState.Chasing:

                if (target == null)
                {
                    ChangeState(GhostState.Searching);
                    break;
                }

                MoveTo(target);

                if (HasReachedDestination(target))
                {
                    ChangeState(GhostState.Attacking);
                }

                break;
            case GhostState.Attacking:

                Character character = target.GetComponent<Character>();
                character.Damage(10);

                ChangeState(GhostState.Chasing);

                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

    private void ChangeState(GhostState state)
    {
        currentState = state;
    }

    private void MoveTo(Transform destination)
    {
        Vector3 directionNormalized = (destination.position - transform.position).normalized;

        transform.Translate(speed * Time.deltaTime * directionNormalized);
    }

    private bool HasReachedDestination(Transform destination)
    {
        float distanceToDestination = Vector3.Distance(transform.position, destination.position);

        return distanceToDestination < 0.5f;
    }

    private Transform GetTarget()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, searchRadius);

        if (collider != null && (collider.CompareTag("Player") || collider.CompareTag("Worker")))
        {
            return collider.transform;
        }

        return null;
    }

    private void GetSearchDestination()
    {
        searchDestination.position = Random.insideUnitSphere * 3f;
    }
}
