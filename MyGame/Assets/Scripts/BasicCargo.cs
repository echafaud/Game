using UnityEngine;
using System;

public class BasicCargo : Unit
{
    public int MaxLives;
    public bool DebuffDash;
    public bool DebuffDoubleJump;
    public float DebuffFallingDamage;
    public float DebuffFallingDistance;
    public float DebuffSpeed;
    public float DebuffJumpForce;
    public float DebuffStamina;
    public int DebuffStaminaRegen;

    public static Action OnRaised;
    public static Action<int, BasicCargo> OnReceivedDamage;
    public static Action<BasicCargo> OnDied;
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player)
        {
            Player.Cargo.Add(this);
            Player.Wallet += cost;
            player.GetDebuff(this);
            gameObject.SetActive(false);
            OnRaised?.Invoke();
            //Debug.Log(Player.Cargo[0].GetType());
        }
    }
    public override void ReceiveDamage(int damage)
    {
        if (!isImmortal)
        {
            OnReceivedDamage?.Invoke(damage, this);
            lives -= damage;


            if (lives < 1)
            {
                lives = 0;
                Die();
            }

        }
    }
    protected override void Die()
    {
        Player.Wallet -= cost;
        OnDied?.Invoke(this);
        //Destroy(gameObject);
    }
}
