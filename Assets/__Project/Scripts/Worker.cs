using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerState { Idle, MovingToPackage, PickingUpPackage, MovingToDeliveryPoint, DeliveringPackage }

public class Worker : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    private Transform deliveryPoint;
    private Transform package;
    private WorkerState currentState;

    private void Awake()
    {
        ChangeState(WorkerState.Idle);
    }

    private void Update()
    {
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
            default:
                break;
        }
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
}
