using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPanelInMenu : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Vector3 startPosition;
    private void Start()
    {
        startPosition = player.position;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player)
        {
            player.transform.position = startPosition;
        }
    }
}
