using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField]
    private float maxTime = 40f;
    private float timeLeft;
    private bool timerActivated = false;
    private Image[] bar;
    private int bonus;
    void Start()
    {
        timeLeft = maxTime;
        bar = GetComponentsInChildren<Image>();
        foreach(var item in bar)
            item.enabled = false;
    }

    void Update()
    {
        if(timerActivated)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                bar[1].fillAmount = timeLeft / maxTime;
            }
            else
            {

                EndTimer();
                //PauseMenu.GameIsPaused = true;
                //loseScreen.SetActive(true);
                //player.ReceiveDamage(int.MaxValue);
            }
        }
    }
    private void StartTimer(int bonus)
    {
        this.bonus = bonus;
        foreach (var item in bar)
            item.enabled = true;
        timerActivated = true;
    }
    private void EndTimer()
    {
        Player.Wallet -= bonus;
        foreach (var item in bar)
            item.enabled = false;
        timerActivated = false;
    }
    private void OnEnable()
    {
        FragilCargo.onRaised += StartTimer;
        FragilCargo.onDied += EndTimer;
    }
    private void OnDisable()
    {
        FragilCargo.onRaised -= StartTimer;
        FragilCargo.onDied -= EndTimer;
    }
}
