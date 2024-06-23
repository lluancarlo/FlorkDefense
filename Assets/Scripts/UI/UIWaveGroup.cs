using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIWaveGroup : MonoBehaviour
{
	public Slider slider;
	public TextMeshProUGUI text;
	public Button button;
	public UnityEvent startNextWaveEvent;
	float _waveDelay;
	Coroutine _nextWaveCoroutine;
	
	void Awake()
	{
		ValidadeProperties();
		gameObject.SetActive(false);
	}

	void ValidadeProperties()
	{
		if (slider == null ) 
			throw new GameExceptions.MissingGameObjectReference(name, typeof(Slider));
		if (text == null ) 
			throw new GameExceptions.MissingGameObjectReference(name, typeof(TextMeshProUGUI));
		if (startNextWaveEvent.GetPersistentEventCount() == 0)
			throw new GameExceptions.MissingEventListener(name, nameof(startNextWaveEvent));
	}

	public void StartCountDown(uint delay)
	{
		_waveDelay = delay;
		gameObject.SetActive(true);
		_nextWaveCoroutine = StartCoroutine(UpdateText());
	}

	IEnumerator UpdateText()
	{
		var delay = _waveDelay;
		while (delay > 0)
		{
			text.text = delay.ToString();
			slider.value = delay / _waveDelay;
			delay--;
			yield return new WaitForSeconds(1f);
		}
		StartNextWave();
	}

	private void StartNextWave()
	{
		text.text = "0";
		gameObject.SetActive(false);
		startNextWaveEvent.Invoke();
		StopCoroutine(_nextWaveCoroutine);
		_nextWaveCoroutine = null;
	}

	public void OnButtonClick() => StartNextWave();
}
