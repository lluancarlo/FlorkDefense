using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthGroup : MonoBehaviour
{
	public Slider slider;
	public float[] steps;

	public void SetHealth(uint health)
	{
		if (health > steps.Length)
			throw new Exception("Health size not defined on steps.");
		var value = steps[health];
		slider.value = value;
	}
}
