using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicMob : Unit
{
    [SerializeField]
    protected int damage;

    protected virtual void Awake() 
    {
        lives = 3;
        damage = 1;
    }
    protected virtual void Update() { }
    protected virtual void Start() { }
    protected virtual void FixedUpdate() { }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player)
            player.ReceiveDamage(damage);
    }
    protected override void Die()
    {
        base.Die();
        Player.Wallet += cost;
    }
}
