using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostState { Idle, Searching, Chasing, Attacking, Dying, Fleeing }

public class Ghost : NPC
{
    [SerializeField]
    private float searchRadius = 2f;
    [SerializeField]
    private float fleeDistance = 10f;

    private Transform target;
    private Transform searchDestination;
    private Transform fleeDestination;
    private GhostState currentState;
    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();

        searchDestination = new GameObject().transform;
        searchDestination.name = "searchDestination";
        GetSearchDestination();
    }

    private void Update()
    {
        if (character.CurrentHitPoints < 0)
        {
            ChangeState(GhostState.Dying);
        }

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
                    Character targetCharacter = target.GetComponent<Character>();

                    if (targetCharacter.HasSprayApplied)
                    {
                        ChangeState(GhostState.Fleeing);
                    }
                    else
                    {
                        ChangeState(GhostState.Chasing);
                    }
                }

                if (HasReachedDestination(searchDestination))
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

                if (target.GetComponent<Character>().HasSprayApplied)
                {
                    ChangeState(GhostState.Fleeing);
                    break;
                }

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
            case GhostState.Dying:

                Destroy(gameObject);

                break;
            case GhostState.Fleeing:

                if (fleeDestination == null)
                {
                    Transform newFleeDestination = new GameObject().transform;
                    newFleeDestination.position = (transform.position - target.position) * fleeDistance;

                    fleeDestination = newFleeDestination;
                }

                MoveTo(fleeDestination);

                if (HasReachedDestination(fleeDestination))
                {
                    Destroy(fleeDestination.gameObject);
                    fleeDestination = null;
                    ChangeState(GhostState.Searching);
                }

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
        var position = pathfinder.GetRandomPositionInsideNavGraph(transform.position, 1);

        searchDestination.position += position;
    }
}
