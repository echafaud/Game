using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MoveableMob : BasicMob
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected Transform leftBorder;
    [SerializeField]
    protected Transform rightBorder;
    [SerializeField]
    private float idleDuration;

    protected bool movingLeft;
    protected bool isIdle;
    private float idleMoveTimer;


    protected override void Awake()
    {
        lives = 2;
        speed = 2.5f;
        movingLeft = true;
    }

    protected override void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (movingLeft)
        {
            if (transform.position.x -1 >= leftBorder.position.x)
            {
                idleMoveTimer = 0;
                transform.position = Vector3.MoveTowards(transform.position, transform.position - transform.right, speed * Time.deltaTime);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                ChangeDirection();
            }
        }

        else
        {
            if (transform.position.x + 1 <= rightBorder.position.x)
            {
                idleMoveTimer = 0;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.right, speed * Time.deltaTime);
                transform.localScale = Vector3.one;
            }
            else
            {
                ChangeDirection();
            }
        }
    }
    protected virtual void ChangeDirection()
    {
        idleMoveTimer += Time.deltaTime;
        isIdle = true;
        if (idleMoveTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            isIdle = false;
        }

    }
}
