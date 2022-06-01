using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CargoBar : MonoBehaviour
{
    private Transform[] cargo = new Transform[2];

    private void Awake()
    {
        for (int i = 0; i < cargo.Length; i++)
        {
            cargo[i] = transform.GetChild(i);
        }
    }

    private void RefreshCargo()
    {
        foreach (var element in Player.Cargo)
        {
            //Debug.Log(element);
            if (element is BigCargo && !cargo[0].gameObject.activeSelf)
                cargo[0].gameObject.SetActive(true);
            else if (element is FragilCargo && !cargo[1].gameObject.activeSelf)
                cargo[1].gameObject.SetActive(true);
        }

    }
    private void ReceiveDamage(int damage, BasicCargo obj)
    {

        var i = obj is BigCargo ? 0 : 1;
        if(cargo[i])
        {
            var icons = cargo[i].gameObject.GetComponentsInChildren<Image>();
            icons[1].fillAmount += ((float)damage / obj.MaxLives);
        }
            
        //Debug.Log((float)damage/ obj.maxLives);
    }
    private void OnEnable()
    {
        BasicCargo.OnRaised += RefreshCargo;
        BasicCargo.OnReceivedDamage += ReceiveDamage;
    }
    private void OnDisable()
    {
        BasicCargo.OnRaised -= RefreshCargo;
        BasicCargo.OnReceivedDamage += ReceiveDamage;
    }
}
