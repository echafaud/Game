using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMob : BasicMob
{
    [SerializeField] 
    private float range;
    [SerializeField]
    private float colliderDistance;
    [SerializeField]
    private float coolDown;
    [SerializeField]
    private float distance;
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private LayerMask player;
    [SerializeField]
    private bool periodicShooting;

    private Collider2D playerInLargeSight;
    private Bullet bullet;
    private SpriteRenderer sprite;
    private bool isRecharged;
    private bool isDead;
    private bool isShooting;
    private Animator animator;
    private WormState State
    {
        get { return (WormState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }

    }
    protected override void Awake()
    {
        lives = 4;
        damage = 2;
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");
        isRecharged = true;
    }
    protected override void FixedUpdate()
    {
        CheckPlayerInLargeSight();
        if (playerInLargeSight)
            ChangeDirection();
    }
    protected override void Update() 
    {
        if(!isDead)
        {
            if(!isShooting)
                State = WormState.Idle;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (isRecharged && (periodicShooting || playerInLargeSight))
        {
            isShooting = true;
            State = WormState.Shoot;
            isRecharged = false;
            StartCoroutine(AttackCoolDown());
        }
    }
    private void OnShoot()
    {
        var position = transform.position;
        position.y += 0.7f;
        position.x += 1.1f * transform.localScale.x / 1.5f;
        var newBullet = Instantiate(bullet, position, bullet.transform.rotation);
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * transform.localScale.x;
        newBullet.Distance = distance;
        newBullet.Damage = damage;
        isShooting = false;
    }
    private void ChangeDirection()
    {
        if (playerInLargeSight.transform.position.x > transform.position.x)
            transform.localScale = Vector3.one * 1.5f;
        else
            transform.localScale = new Vector3(-1.5f, 1.5f, 1); 
       
    }
    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(coolDown);
        isRecharged = true;
    }
    private void CheckPlayerInLargeSight()
    {
        playerInLargeSight = Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, player);
    }

    protected override void Die()
    {
        isDead = true;
        State = WormState.Die;
    }
    private void OnDie()
    {
        base.Die();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

}
public enum WormState
{
    Idle,
    Shoot,
    Die
}
