using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Unit
{
    public static List<BasicCargo> Cargo = new List<BasicCargo>();
    public static Action OnGround;
    public static int Wallet = 0;
    public static int ShootDamage = 1;
    public static int MeleeAttackDamage = 2;
    public static float Stamina = 100;
    public static int MaxLives = 25;
    
    [SerializeField] 
    private float speed = 4f;
    [SerializeField] 
    private float jumpForce = 1.5f;
    [SerializeField] 
    private float dashSpeed = 15f;
    [SerializeField]
    private int staminaRegen = 30;
    [SerializeField]
    private int dashCost = 40;
    [SerializeField]
    private int jumpCost = 20;
    [SerializeField]
    private int doubleJumpCost = 10;
    [SerializeField]
    private int shootCost = 10;
    [SerializeField] 
    private float attackRange;
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private float attackColliderDistance;
    [SerializeField]
    private LayerMask enemy;
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private StaminaBar staminaBar;
    [SerializeField]
    private float fallingThreshold;
    [SerializeField]
    private GameObject deathScreen;

    private bool isDead;
    private bool canMove;
    private bool isAttacking;
    private bool debuffDash;
    private bool debuffDoubleJump;
    private bool isFalling;
    private bool canDash;
    private bool hasRegeneretedStamina;
    private bool isDashing;
    private bool isJumping;
    private bool isDoubleJumping;
    private bool isShooting;
    private bool canDoubleJump;
    private bool isGrounded;
    private bool isRecharged;
    private bool isJumpReload;
    private float startSeparationGround;

    private float debuffFalling;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Bullet bullet;
    private Animator animator;

    private PlayerState State
    {
        get { return (PlayerState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }

    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");
        animator = GetComponent<Animator>();
        lives = MaxLives;
        MaxLives = lives;
        fallingThreshold = 5;
        debuffDash = false;
        debuffDoubleJump = false;
        debuffFalling = 1;
        isRecharged = true;
        canMove = true;
        canDash = true;
        canDoubleJump = true;
        hasRegeneretedStamina = true;
        isDashing = false;
        isJumping = false; 
        isDoubleJumping = false; 
        isShooting = false;
        isDead = false;
        healthBar.SetMaxHealth(lives);
        staminaBar.SetMaxStamina(Stamina);
    }

    private void Update()
    {
        if (!isDead)
        {
            if (isGrounded && !isAttacking && !isShooting)
                State = PlayerState.Idle;
            else if (isJumpReload)
                State = PlayerState.JumpReload;
            else if (isJumping)
                State = PlayerState.Jump;
            else if (isDoubleJumping)
                State = PlayerState.Jump;
            else if (isFalling)
                State = PlayerState.Falling;
            
            if (Input.GetButtonDown("Fire1"))
                Shoot();
            if (Input.GetButtonDown("Fire2"))
                Attack();
            if (Input.GetButtonDown("Fire3"))
                Dash();
            if (Input.GetButton("Horizontal"))
                Move();
            if (Input.GetButtonDown("Jump"))
                Jump();

            healthBar.SetHealth(lives);
            staminaBar.SetStamina(Stamina);

            if (!(isDashing || isJumping || isDoubleJumping || isShooting) && !hasRegeneretedStamina)
            {
                StaminaRegen();
            }
        }
        if (PauseMenu.GameIsPaused)
            State = PlayerState.Idle;
    }

    private void FixedUpdate()
    {

        CheckGround();
        if (isGrounded)
        {
            OnGround?.Invoke();
            isDashing = false;
            isDoubleJumping = false;

            if (isFalling)
            {
                isFalling = false;
                if (transform.position.y < startSeparationGround - fallingThreshold)
                    ApplyFallingDamage(startSeparationGround - transform.position.y);
            }

        }
        else
        {
            if ((isJumping || isDoubleJumping) && rb.velocity.y > 0)
                isFalling = false;

            else if ((isJumping || isDoubleJumping) && rb.velocity.y < 0)
            {
                if (rb.velocity.y > -0.6)
                {
                    isFalling = false;
                    isJumpReload = true;
                }
                else
                {
                    isJumpReload = false;
                    isJumping = false;
                    isDoubleJumping = false;
                    isFalling = true;
                }

            }

            else if (!isJumping && !isFalling)
            {
                isFalling = true;
                startSeparationGround = transform.position.y;
            }


        }
        CheckStamina();
    }

    private void Move()
    {
        if (canMove)
        {
            var dir = transform.right * Input.GetAxis("Horizontal");
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
            if (dir.x < 0f)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = Vector3.one;

            if (isGrounded)
                State = PlayerState.Run;

        }
    }

    private void Jump()
    {
        if (isGrounded && Stamina >= jumpCost)
        {
            isJumping = true;
            startSeparationGround = transform.position.y;
            Stamina -= jumpCost;
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            canDoubleJump = true;
            isAttacking = false;
            canMove = true;
        }
        else if (canDoubleJump && !isDashing && Stamina > doubleJumpCost && !debuffDoubleJump && rb.velocity.y < 0)
        {
            isDoubleJumping = true;
            startSeparationGround = transform.position.y;
            isJumping = false;
            Stamina -= doubleJumpCost;
            rb.velocity = Vector2.zero;
            rb.AddForce(transform.up * jumpForce * 0.8f, ForceMode2D.Impulse);
            canDoubleJump = false;
        }
    }

    private void Dash()
    {
        if(canDash && Stamina >= dashCost && !debuffDash)
        {
            isDashing = true;
            isImmortal = true;
            Stamina -= dashCost;

            rb.velocity = Vector2.zero;
            rb.velocity = Vector2.right * transform.localScale.x * dashSpeed;
            canDash = false;
            StartCoroutine(StopDashing());
            StartCoroutine(DashCoolDown());
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(0.15f);
        isImmortal = false;
        rb.velocity = Vector2.zero;
        if (isGrounded)
            rb.velocity = Vector2.zero;
    }
    private IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(1f);
        canDash = true;
    }

    private void Shoot()
    {
        if (isGrounded && isRecharged && Stamina >= shootCost)
        {
            canMove = false;
            isRecharged = false;
            isShooting = true;
            State = PlayerState.Shoot;

            Stamina -= shootCost;
            StartCoroutine(AnimationShoot());
            StartCoroutine(AttackCoolDown());
        }
    }
    private void OnShoot()
    {
        var position = transform.position;
        position.y += 0.7f;
        position.x += 0.3f*transform.localScale.x;
        var newBullet = Instantiate(bullet, position, bullet.transform.rotation);
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * transform.localScale.x;
        newBullet.Distance = 1.5f;
        newBullet.Damage = ShootDamage;
    }
    private IEnumerator AnimationShoot()
    {
        yield return new WaitForSeconds(0.4f);
        isShooting = false;
        canMove = true;
    }
    private void Attack()
    {
        if (isRecharged && isGrounded)
        {
            canMove = false;
            State = PlayerState.Attack;
            isRecharged = false;
            isAttacking = true;
            StartCoroutine(AttackCoolDown());
        }
    }
    private void OnAttack()
    {
        var enemies = Physics2D.OverlapBoxAll(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * attackColliderDistance,
                new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, enemy);
        for (var i = 0; i < enemies.Length; i++)
            enemies[i].GetComponent<Unit>().ReceiveDamage(MeleeAttackDamage);
        isAttacking = false;
        canMove = true;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    protected override void Die()
    {
        isDead = true;
        State = PlayerState.Die;
        for (var i = 0; i < Cargo.Count; i++)
        {
            if (Cargo[i])
            {
                Cargo[i].ReceiveDamage(int.MaxValue);
            }

        }

        healthBar.SetHealth(0);
    }
    private void OnDie()
    {
        deathScreen.SetActive(true);
        Destroy(gameObject);
    }
    public void GetDebuff(BasicCargo cargo)
    {
        debuffDash = cargo.DebuffDash;
        debuffDoubleJump = cargo.DebuffDoubleJump;
        debuffFalling = cargo.DebuffFallingDamage;
        fallingThreshold -= cargo.DebuffFallingDistance;
        speed -= cargo.DebuffSpeed;
        jumpForce -= cargo.DebuffJumpForce;
        Stamina -= cargo.DebuffStamina;
        staminaRegen -= cargo.DebuffStaminaRegen;
    }
    public void CargoReceiveGamage(int damage)
    {
        if(!isImmortal)
        {
            var tempCount = Cargo.Count;
            for (var i = 0; i < Cargo.Count; i++)
            {
                Cargo[i].ReceiveDamage(damage);
                if (tempCount > Cargo.Count)
                {
                    tempCount--;
                    i--;
                }

            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * attackColliderDistance,
                new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void OnEnable()
    {
        BasicCargo.OnDied += RefreshCargo;
        BasicCargo.OnDied += RemoveDebuff;
    }
    private void OnDisable()
    {
        BasicCargo.OnDied -= RefreshCargo;
        BasicCargo.OnDied -= RemoveDebuff;
    }
    private void RemoveDebuff(BasicCargo cargo)
    {
        debuffDash = false;
        debuffDoubleJump = false;
        debuffFalling = 1;
        fallingThreshold += cargo.DebuffFallingDistance;
        speed += cargo.DebuffSpeed;
        jumpForce += cargo.DebuffJumpForce;
        Stamina += cargo.DebuffStamina;
        staminaRegen += cargo.DebuffStaminaRegen;
    }
    private void RefreshCargo(BasicCargo cargo)
    {
        Cargo.Remove(cargo);
        RemoveDebuff(cargo);
    }
    private void RefreshWallet(int cost)
    {
        Wallet += cost;
    }    
    private void CheckStamina()
    {
        hasRegeneretedStamina = Stamina >= 100;
/*        stamina = hasRegeneretedStamina ? 100 : stamina;*/
    }
    private void CheckGround()
    {
        var collider = Physics2D.OverlapCircleAll(transform.position, 0.3f, ground);
        isGrounded = collider.Length > 0;
        
    }
    private void ApplyFallingDamage(float distanse)
    {
        CargoReceiveGamage(1);
        ReceiveDamage((int)(distanse*debuffFalling));
    }
    private void StaminaRegen()
    {
        Stamina += staminaRegen * Time.deltaTime;
        //Debug.Log(stamina);
    }
}

public enum PlayerState
{
    Idle,
    Run,
    Jump,
    JumpReload,
    Falling,
    Attack,
    Shoot,
    Die
}