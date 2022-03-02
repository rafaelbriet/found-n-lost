using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimationEventsHandler : MonoBehaviour
{
    [SerializeField]
    private Ghost ghost;

    public void Attack()
    {
        ghost.DoAttack();
    }

    public void Die()
    {
        ghost.DoDie();
    }

    public void Spawn()
    {
        ghost.DoSpawn();
    }
}
