using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	public UIHealthGroup healthGroup;
	public UIWaveGroup waveGroup;
	public TextMeshProUGUI moneyText;
	public GameObject towerMenu;

	void Awake()
	{
		if (Instance != null)
			return;
		Instance = this;
		ValidadeProperties();
		towerMenu.SetActive(false);
	}

	void ValidadeProperties()
	{
		if (moneyText == null )
			throw new GameExceptions.MissingGameObjectReference(name, typeof(TextMeshProUGUI));
		if (healthGroup == null )
			throw new GameExceptions.MissingGameObjectReference(name, typeof(UIHealthGroup));
		if (waveGroup == null )
			throw new GameExceptions.MissingGameObjectReference(name, typeof(UIWaveGroup));
		if (towerMenu == null )
			throw new GameExceptions.MissingGameObjectReference(name, typeof(GameObject));
	}

	public void WaveBarStartCountDown(uint time)
	{
		waveGroup.StartCountDown(time);
	}

	public void OnTowerMenuSelected(int id)
	{
		GameManager.Instance.OnTowerBuild(id);
	}

	public void UpdateMoney(uint money)
	{
		moneyText.text = money.ToString();
	}

	public void UpdateLife(uint life)
	{
		healthGroup.SetHealth(life);
	}

	public void OnStartNewWave()
	{
		GameManager.Instance.StartNextWave();
	}
}
