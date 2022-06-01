using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FinishTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject finishMenuUI;
    [SerializeField]
    private TextMeshProUGUI score;

    private int startWallet;
    private void Start()
    {
        startWallet = Player.Wallet;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player)
        {
            PauseMenu.GameIsPaused = true;
            Time.timeScale = 0;
            score.text = "You have collected " + (Player.Wallet - startWallet).ToString() + " coins";
            finishMenuUI.SetActive(true);
        }
    }
    public void GoNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
