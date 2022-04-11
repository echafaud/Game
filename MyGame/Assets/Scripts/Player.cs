using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] float speed = 3f;
    [SerializeField] float jumpForce = 1.4f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {

        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
    }

    private void Run()
    {
        var dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        CheckGround();
    }


    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;
    }
}

