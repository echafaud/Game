using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject shopMenuUI;
    [SerializeField]
    private GameObject mainMenuUI;
    [SerializeField]
    private TextMeshProUGUI fireballDamage;
    [SerializeField]
    private TextMeshProUGUI melleAttacklDamage;
    [SerializeField]
    private TextMeshProUGUI health;
    [SerializeField]
    private TextMeshProUGUI stamina;

    private int fireballCost = 50;
    private int healtCost = 100;
    private int staminaCost = 70;
    private int attackCost = 150;
    void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PauseMenu.GameIsPaused)
            {
                Resume();
            }
            else
            {
                ActivateShop();
            }
        }

    }
    public void ActivateShop()
    {
        shopMenuUI.SetActive(true);
        mainMenuUI.SetActive(false);
        Time.timeScale = 0;
        PauseMenu.GameIsPaused = true;
    }
    public void Resume()
    {
        shopMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
        Time.timeScale = 1;
        PauseMenu.GameIsPaused = false;
    }
    public void BuyFireball()
    {
        if (Player.Wallet >= fireballCost)
        {
            Player.Wallet -= fireballCost;
            Player.ShootDamage += 2;
            fireballDamage.text = Player.ShootDamage.ToString();
        }
    }
    public void BuyStamina()
    {
        if (Player.Wallet >= staminaCost)
        {
            Player.Wallet -= staminaCost;
            Player.Stamina += 10;
            stamina.text = Player.Stamina.ToString();
        }
    }
    public void BuyHealt()
    {
        if (Player.Wallet >= healtCost)
        {
            Player.Wallet -= healtCost;
            Player.MaxLives += 5;
            health.text = Player.MaxLives.ToString();
        }
    }
    public void BuyMeleeAttack()
    {
        if (Player.Wallet >= attackCost)
        {
            Player.Wallet -= attackCost;
            Player.MeleeAttackDamage += 5;
            melleAttacklDamage.text = Player.MeleeAttackDamage.ToString();
        }
    }
}
