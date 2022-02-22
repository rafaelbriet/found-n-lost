using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerState { Idle, MovingToPackage, PickingUpPackage, MovingToDeliveryPoint, DeliveringPackage, Fleeing }

public class Worker : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    private Transform deliveryPoint;
    private Transform package;
    private Transform fleeSpot;
    private WorkerState currentState;
    private float searchRadius = 3f;

    private void Awake()
    {
        ChangeState(WorkerState.Idle);
    }

    private void Update()
    {
        if (currentState != WorkerState.Fleeing && IsGhostNear())
        {
            Debug.Log("Check for ghosts");
            ChangeState(WorkerState.Fleeing);
        }

        switch (currentState)
        {
            case WorkerState.Idle:

                deliveryPoint = GetDeliveryPoint();
                package = GetPackage();

                if (package != null)
                {
                    ChangeState(WorkerState.MovingToPackage);
                }

                break;
            case WorkerState.MovingToPackage:

                MoveTo(package);

                if (HasReachedDestination(package))
                {
                    ChangeState(WorkerState.PickingUpPackage);
                }

                break;
            case WorkerState.PickingUpPackage:

                package.SetParent(transform);
                package.position = new Vector3(0, 0, 0);
                package.localPosition = new Vector3(0, 0.75f, 0);

                ChangeState(WorkerState.MovingToDeliveryPoint);

                break;
            case WorkerState.MovingToDeliveryPoint:

                MoveTo(deliveryPoint);

                if (HasReachedDestination(deliveryPoint))
                {
                    ChangeState(WorkerState.DeliveringPackage);
                }

                break;
            case WorkerState.DeliveringPackage:

                Destroy(package.gameObject);
                package = null;

                ChangeState(WorkerState.Idle);

                break;
            case WorkerState.Fleeing:
                
                if (fleeSpot == null)
                {
                    fleeSpot = GetFleeSpot();
                }

                if (package != null)
                {
                    package.SetParent(null);
                    package = null;
                }

                MoveTo(fleeSpot);

                if (HasReachedDestination(fleeSpot))
                {
                    ChangeState(WorkerState.Idle);
                }

                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

    private void ChangeState(WorkerState state)
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

    private bool IsGhostNear()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, searchRadius, 1 << LayerMask.NameToLayer("Enemy"));

        if (collider != null && collider.CompareTag("Ghost"))
        {
            return true;
        }

        return false;
    }

    private Transform GetDeliveryPoint()
    {
        GameObject deliveryPoint = GameObject.FindGameObjectWithTag("Delivery Point");

        return deliveryPoint != null ? deliveryPoint.transform : null;
    }

    private Transform GetPackage()
    {
        GameObject package = GameObject.FindGameObjectWithTag("Package");

        return package != null ? package.transform : null;
    }

    private Transform GetFleeSpot()
    {
        GameObject package = GameObject.FindGameObjectWithTag("Flee Spot");

        return package != null ? package.transform : null;
    }
}
