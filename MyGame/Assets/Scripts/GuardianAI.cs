using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GuardianAI : BasicMob
{
    [SerializeField]
    private CircleCollider2D AggressionArea;
    [SerializeField]
    private float radius;
    [SerializeField]
    private LayerMask player;
    [SerializeField]
    private GameObject protectedCargo;

    private bool isSleeping;
    private bool isDead;
    private AIDestinationSetter pursueScript;
    private AIPath pathScript;
    private Animator animator;
    private Vector3 startPosition;

    private BatState State
    {
        get { return (BatState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }

    }
    protected override void Start()
    {

        isSleeping = true;
        animator = GetComponent<Animator>();
        pursueScript = GetComponent<AIDestinationSetter>();
        pathScript = GetComponent<AIPath>();
        startPosition = transform.position;
    }
    protected override void FixedUpdate()
    {
        if (!isDead)
        {
            if (!isSleeping)
            {
                State = BatState.Fly;
                if (Physics2D.OverlapCircle(AggressionArea.bounds.center, radius, player) || !protectedCargo.activeSelf)
                {
                    pursueScript.enabled = true;
                    pathScript.enabled = true;

                }
                else
                {
                    pursueScript.enabled = false;
                    pathScript.enabled = false;
                    transform.position = Vector3.MoveTowards(transform.position, startPosition, 3 * Time.deltaTime);
                }
            }

            else if (Physics2D.OverlapCircle(AggressionArea.bounds.center, radius, player) || !protectedCargo.activeSelf)
            {
                State = BatState.StartToFly;
            }
        }
        else
        {
            pursueScript.enabled = false;
            pathScript.enabled = false;
        }
    }
    private void OnAwakeBat()
    {
        isSleeping = false;
    }
    protected override void Die()
    {
        isDead = true;
        State = BatState.Die;
    }
    private void OnDie()
    {
        base.Die();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AggressionArea.bounds.center, radius);
    }
}
public enum BatState
{
    Idle,
    StartToFly,
    Fly,
    Attack,
    Die
}