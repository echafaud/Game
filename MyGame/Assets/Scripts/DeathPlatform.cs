using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    //private float yPosition;

/*    private void Start()
    {
        transform.position = new Vector3(player.position.x, -100, 0);
    }*/

/*    void FixedUpdate()
    {
        if(player)
        {
            Debug.Log(1);
            transform.position = new Vector3(player.position.x, -20, 0);
        }

    }*/
    /*private void SetYPosition()
    {
        yPosition = player.position.y - 15;
    }*/

    /*private void OnEnable()
    {
        Player.OnGround += SetYPosition;
    }
    private void OnDisable()
    {
        Player.OnGround -= SetYPosition;
    }*/
    private void OnTriggerEnter2D(Collider2D collider)
    {

        var player = collider.GetComponent<Player>();
        if (player)
        {
            player.ReceiveDamage(int.MaxValue);
        }
    }
}
