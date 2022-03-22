using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GhostState { Spawning, Idle, Searching, Chasing, Attacking, Dying, Fleeing }

[RequireComponent(typeof(Character), typeof(Collider2D))]
public class Ghost : NPC
{
    [SerializeField]
    private float attackRange = 3f;
    [SerializeField]
    private float attackCooldown = 2f;
    [SerializeField]
    private float minSearchRadius = 2f;
    [SerializeField]
    private float maxSearchRadius = 10f;
    [SerializeField]
    private int minAttack = 10;
    [SerializeField]
    private int maxAttack = 20;
    [SerializeField]
    private int fleeDistance = 5;
    [SerializeField]
    private LayerMask layerMask;

    [Header("Debug")]
    [SerializeField]
    private bool showCurrentState;
    [SerializeField]
    private GameObject debugCanvas;
    [SerializeField]
    private TextMeshProUGUI currentStateText;

    private GhostState currentState;
    private Character target;
    private Character character;
    private Vector3 destination;
    private bool canAttack = true;
    private new Collider2D collider2D;
    private float currentSearchRadius;
    private int currentAttack;

    private void Awake()
    {
        character = GetComponent<Character>();
        collider2D = GetComponent<Collider2D>();

        currentAttack = minAttack;
        currentSearchRadius = minSearchRadius;

        UpdateStateDebugText();
    }

    private void Update()
    {
        if (character.CurrentHitPoints <= 0)
        {
            ChangeState(GhostState.Dying);
        }

        UpdateState();
        
        debugCanvas.SetActive(showCurrentState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, currentSearchRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void IncreaseSearchRadius(float amount)
    {
        currentSearchRadius = Mathf.Clamp(currentSearchRadius + amount, minSearchRadius, maxSearchRadius);
    }

    public void IncreaseAttackDamage(int amount)
    {
        currentAttack = Mathf.Clamp(currentAttack + amount, minAttack, maxAttack);
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case GhostState.Spawning:
                Spawn();
                break;
            case GhostState.Idle:
                Idle();
                break;
            case GhostState.Searching:
                Search();
                break;
            case GhostState.Chasing:
                Chase();
                break;
            case GhostState.Attacking:
                Attack();
                break;
            case GhostState.Dying:
                Die();
                break;
            case GhostState.Fleeing:
                Flee();
                break;
            default:
                break;
        }
    }

    private void Flee()
    {
        DoMoveAnimation("Ghost", "Walk");

        MoveTo(destination);

        if (HasReachedDestination())
        {
            ChangeState(GhostState.Searching);
        }
    }

    private void Die()
    {
        animator.Play("Base Layer.Ghost_Death");
        collider2D.enabled = false;
    }

    public void DoDie()
    {
        gameObject.SetActive(false);
    }

    private void Attack()
    {
        if (IsInAttackRange() == false)
        {
            ChangeState(GhostState.Chasing);
            return;
        }

        if (canAttack && IsInAttackRange())
        {
            animator.Play($"Base Layer.Ghost_Attack_Front");
        }
    }

    public void DoAttack()
    {
        if (canAttack && IsInAttackRange())
        {
            target.Damage(currentAttack);
            StartCoroutine(AttackCooldownCoroutine());
        }
    }

    private IEnumerator AttackCooldownCoroutine()
    {
        canAttack = false;

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    private void Chase()
    {
        if (target == null)
        {
            GetSearchDestination();
            ChangeState(GhostState.Searching);
        }
        else if (target.HasSprayApplied)
        {
            GetFleeDestination();
            ChangeState(GhostState.Fleeing);
        }
        else if (IsInAttackRange())
        {
            ChangeState(GhostState.Attacking);
        }
        else
        {
            DoMoveAnimation("Ghost", "Chase");
            
            destination = target.transform.position;

            MoveTo(destination);
        }
    }

    private void Search()
    {
        DoMoveAnimation("Ghost", "Walk");

        MoveTo(destination);

        if (HasReachedDestination())
        {
            GetSearchDestination();
        }

        if (HasFoundCharacter(out Character characterFound))
        {
            target = characterFound;

            if (characterFound.HasSprayApplied)
            {
                GetFleeDestination();
                ChangeState(GhostState.Fleeing);
            }
            else
            {
                destination = characterFound.transform.position;
                ChangeState(GhostState.Chasing);
            }
        }
    }

    private void Idle()
    {
        animator.Play("Base Layer.Ghost_Idle_Front");

        GetSearchDestination();

        ChangeState(GhostState.Searching);
    }

    private void Spawn()
    {
        animator.Play("Base Layer.Ghost_Spawn");
    }

    public void DoSpawn()
    {
        ChangeState(GhostState.Idle);
    }

    private void ChangeState(GhostState state)
    {
        currentState = state;

        UpdateStateDebugText();
    }

    private void UpdateStateDebugText()
    {
        currentStateText.text = currentState.ToString();
    }

    private bool HasReachedDestination()
    {
        float distanceToDestination = Vector3.Distance(transform.position, destination);

        return distanceToDestination < 0.5f;
    }

    private bool IsInAttackRange()
    {
        if (target == null)
        {
            return false;
        }

        float distanceToDestination = Vector3.Distance(transform.position, target.transform.position);

        return distanceToDestination < attackRange;
    }

    private bool HasFoundCharacter(out Character character)
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, currentSearchRadius, layerMask);

        if (collider != null)
        {
            character = collider.GetComponent<Character>();
            return true;
        }

        character = null;
        return false;
    }

    private void GetSearchDestination()
    {
        destination = pathfinder.GetRandomPositionInsideNavGraph(transform.position, (int)currentSearchRadius);
    }

    private void GetFleeDestination()
    {
        Vector3 fleeDirection = transform.position - target.transform.position;
        destination =  pathfinder.GetPositionInsideNavGraph(transform.position, fleeDirection, fleeDistance);
    }
}
