using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMob : MoveableMob
{
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float colliderDistance;
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private LayerMask player;

    private Collider2D playerInShortSight;
    private Animator animator;
    private bool canMove;
    private bool isRecharged;
    private bool isDead;
    private SlimeState State
    {
        get { return (SlimeState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }

    }
    protected override void Awake()
    {
        cost = 1;
        lives = 4;
        damage = 2;
        animator = GetComponent<Animator>();
        isRecharged = true;
    }

    protected override void Update()
    {
        if (!isDead)
        {
            if (canMove)
            {
                Move();
            }

            if (isIdle)
            {
                State = SlimeState.Idle;
            }
            else if (playerInShortSight)
            {
                Attack();
            }
        }
    }
    protected override void FixedUpdate()
    {
        CheckPlayerInShortSight();
        canMove = !playerInShortSight;

    }
    protected override void Move()
    {
        base.Move();
        State = SlimeState.Move;
    }
    private void Attack()
    {
        if (isRecharged)
        {
            State = SlimeState.Attack;
            isRecharged = false;
            StartCoroutine(AttackCoolDown());
        }
    }
    private void OnAttack()
    {
        var player = playerInShortSight.GetComponent<Player>();
        player.ReceiveDamage(damage);
        player.CargoReceiveGamage(1);
    }
    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(2f);
        isRecharged = true;
    }

    protected override void Die()
    {
        isDead = true;
        State = SlimeState.Die;
    }
    private void OnDie()
    {
        base.Die();
    }

    private void CheckPlayerInShortSight()
    {
        playerInShortSight = Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, player);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
public enum SlimeState
{
    Idle,
    Move,
    Attack,
    Die
}
