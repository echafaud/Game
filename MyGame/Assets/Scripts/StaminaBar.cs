using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaBar : MonoBehaviour
{
	public Slider slider;
	public Image fill;

	public void SetMaxStamina(float stamina)
	{
		slider.maxValue = stamina;
		slider.value = stamina;

	}

	public void SetStamina(float stamina)
	{
		/*if (stamina < slider.value)
			slider.value -=  (slider.value -stamina) * Time.deltaTime;*/
		slider.value = stamina;
    }
}
