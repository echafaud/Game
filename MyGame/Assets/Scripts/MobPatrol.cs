using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobPatrol : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;
    [SerializeField] 
    private Transform leftBorder;
    [SerializeField] 
    private Transform rightBorder;
    [SerializeField] 
    private Transform enemy;

    private bool movingLeft;
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        enemy.position = Vector3.MoveTowards(enemy.position, enemy.position + enemy.right * GetDirection(), speed * Time.deltaTime);
    }
    private int GetDirection()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftBorder.position.x)
                return -1;
            else
            {
                movingLeft = !movingLeft;
                return 1;
            }
                
        }
        else
        {
            if (enemy.position.x <= rightBorder.position.x)
                return 1;
            else
            {
                movingLeft = !movingLeft;
                return -1;
            }
        }
    }
}
