using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private Transform player;

    private void Update()
    {
        if(player)
        {
            var position = player.position;
            position.z = -10.0F;
            transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
        }
    }
}
