using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected int cost;
    [SerializeField] 
    protected int lives;
    [SerializeField]
    protected bool isImmortal;
    [SerializeField] 
    private Flash flashEffect;
    public virtual void ReceiveDamage()
    {
        if (!isImmortal)
        {
            lives--;
            flashEffect.DoFlash();
            if (lives < 1)
            {
                lives = 0;
                Die();
            }
        }     
    }
    public virtual void ReceiveDamage(int damage)
    {
        if(!isImmortal)
        {
            lives -= damage;
            flashEffect.DoFlash();
            Debug.Log(lives);
            if (lives < 1)
            {
                lives = 0;
                Die();
            }
        }
    }

    protected virtual void Die()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
