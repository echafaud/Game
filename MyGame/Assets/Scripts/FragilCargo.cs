using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FragilCargo : BasicCargo
{
    public static new Action<int> onRaised;
    public static new Action onDied;
    private int bonus = 100;
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        if(collider.GetComponent<Player>())
        {
            Player.Wallet += bonus;
            onRaised?.Invoke(bonus);
        }

    }
    protected override void Die()
    {
        base.Die();
        onDied?.Invoke();
    }
}
