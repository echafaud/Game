using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalletManagerUI : MonoBehaviour
{
    private TextMeshProUGUI coinsText;
    void Start()
    {
        coinsText = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(coinsText);
    }

    void Update()
    {
        coinsText.text = Player.Wallet.ToString();
    }
}
