using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiMob : MoveableMob
{
    [SerializeField]
    private float dashSpeed = 80f;
    [SerializeField]
    private int dashDamage = 5;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float sightRange;
    [SerializeField]
    private float colliderDistance;
    [SerializeField]
    private float attackColliderDistance;
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private LayerMask player;

    private Collider2D playerInShortSight;
    private Collider2D playerInLargeSight;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isRecharged;
    private bool isDashing;
    private bool canDash;
    private  bool canMove;
    private bool isDead;
    private SamuraiState State
    {
        get { return (SamuraiState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }

    }
    protected override void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();    
        canDash = true;
        isRecharged = true;
        canMove = true;
        movingLeft = true;
        lives = 5;
        speed = 4;
        damage = 2;
        StartCoroutine(StartDash());
    }
    protected override void Update()
    {
        if (!isDead)
        {
            if (canMove)
            {
                Move();
                State = SamuraiState.Move;
            }
            if (isDashing)
                State = SamuraiState.Dash;
            else if (playerInShortSight)
            {
                Attack();
            }
            else if (isIdle)
            {
                State = SamuraiState.Idle;
            }
        }
    }
    protected override void FixedUpdate()
    {
        CheckPlayerInShortSight();
        CheckPlayerInLargeSight();
        CheckBeyondBorders();
        if (playerInShortSight || isDashing)
            canMove = false;
        else
            canMove = true;

    }
    private IEnumerator StartDash()
    {
        while (true)
        {
            if (playerInLargeSight && canDash)
            {
                isDashing = true;
                isImmortal = true;
                canDash = false;
            }
            yield return null;
        }
    }

    private void Dash()
    {
        rb.velocity = Vector2.right * transform.localScale.x * dashSpeed;
        StartCoroutine(StopDashing());
        StartCoroutine(DashCoolDown());
    }
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(0.4f);
        rb.velocity = Vector2.zero;
        isDashing = false;
        isImmortal = false;
    }
    private IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(8f);
        canDash = true;
    }
    private void Attack()
    {
        if (isRecharged && !isDashing)
        {
            State = SamuraiState.Attack;
            isRecharged = false;
            StartCoroutine(AttackCoolDown());
        }
    }
    private void OnAttack()
    {
        if(playerInShortSight)
        {
            var player = playerInShortSight.GetComponent<Player>();
            player.ReceiveDamage(damage);
            player.CargoReceiveGamage(1);

        }
    }
    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(2f);
        isRecharged = true;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * sightRange * transform.localScale.x * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * sightRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * attackColliderDistance,
                new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player && isDashing)
        {
            player.ReceiveDamage(dashDamage);
            player.CargoReceiveGamage(2);
        }
    }

    protected override void Die()
    {
        isDead = true;
        State = SamuraiState.Die;
    }

    private void OnDie()
    {
        base.Die();
    }
    private void CheckPlayerInLargeSight()
    {
        playerInLargeSight = Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * sightRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * sightRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, player);
    }
    private void CheckPlayerInShortSight()
    {
        playerInShortSight = Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * attackColliderDistance,
                new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, player);   
    }
    private void CheckBeyondBorders()
    {
        if ((transform.position.x -1 <= leftBorder.position.x || transform.position.x+1 >= rightBorder.position.x) && isDashing)
        {
            rb.velocity = Vector3.zero;
        }

    }
}
public enum SamuraiState
{
    Idle,
    Move,
    Attack,
    Dash,
    Die
}