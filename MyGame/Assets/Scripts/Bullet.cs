using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    private LayerMask ground;
    [SerializeField]
    private LayerMask bullets;

    private bool canMove;
    private Animator animator;
    private float speed = 5f;
    private bool isGrounded;
    private Vector3 direction;
    private SpriteRenderer sprite;
    public GameObject Parent { get; set; }
    public int Damage { get; set; }
    public float Distance { get; set; }
    public Vector3 Direction { set { direction = value; } }

    private void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        canMove = true;

        StartCoroutine(BulletAnimationTimeLeft());

        if (direction.x < 0f)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = Vector3.one;
    }

    private void FixedUpdate()
    {
        if (canMove)
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        CollisionWithBullet();
        CheckGround();
        if (isGrounded)
        {
            canMove = false;
            animator.SetBool("IsExplosion", true);
            StartCoroutine(BulletAnimationExplosion());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var unit = collider.GetComponent<Unit>();
        if (unit && unit.gameObject != Parent)
        {
            canMove = false;
            unit.ReceiveDamage(Damage);

            if (unit is Player player)
            {
                player.CargoReceiveGamage(Damage / 2);
            }
            
            animator.SetBool("IsExplosion", true);
            StartCoroutine(BulletAnimationExplosion());
        }
    }
    private void CollisionWithBullet()
    {
        var bullet = Physics2D.OverlapCircle(transform.position, 0.55f, bullets);
        if (bullet && bullet.GetComponent<Bullet>().Parent != Parent)
        {
            canMove=false;
            animator.SetBool("IsExplosion", true);
            StartCoroutine(BulletAnimationExplosion());
        }
    }
    private void CheckGround()
    {
        var grounds = Physics2D.OverlapCircleAll(transform.position, 0.15f, ground);
        isGrounded = grounds.Length > 0;
    }
    private IEnumerator BulletAnimationExplosion()
    {
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }
    private IEnumerator BulletAnimationTimeLeft()
    {
        yield return new WaitForSeconds(Distance);
        canMove = false;
        animator.SetBool("IsExplosion", true);
        StartCoroutine(BulletAnimationExplosion());
    }
}