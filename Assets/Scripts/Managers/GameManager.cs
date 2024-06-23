using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public GameObject towerMenu;
	public GameObject[] towers;
	public uint waveDelay = 20;
	public uint initialMoney = 10;
	public uint initialLife = 5;
	
	GameState gameState { get; set; }

  TowerSlot _selectedTower = null;
	int _currentWave = -1;

	void Awake()
	{
		if (Instance != null)
			return;
		Instance = this;
		ValidadeProperties();
	}

	void ValidadeProperties()
	{
		if (towerMenu == null)
			throw new GameExceptions.MissingGameObjectReference(name, typeof(GameObject));
		if (towers.Length == 0)
			throw new GameExceptions.MissingGameObjectReference(name, typeof(GameObject[]));
	}

	void Start()
	{
		SetInitialGameState();
		UpdateUIAsGameState();

		UIManager.Instance.WaveBarStartCountDown(waveDelay);
	}

	void SetInitialGameState()
	{
		gameState = new GameState();
		gameState.AddMoney(initialMoney);
		gameState.AddLife(initialLife);
	}
	
	void UpdateUIAsGameState()
	{
		UIManager.Instance.UpdateMoney(gameState.currentMoney);
		UIManager.Instance.UpdateLife(gameState.currentLife);
	}

	public void OnSelect(TowerSlot tower)
	{
    if (_selectedTower != null)
      _selectedTower.Deselect();
    _selectedTower = tower;
    _selectedTower.Select();
		towerMenu.SetActive(true);
	}

	public void OnDeselect()
	{
		if (_selectedTower != null)
		{
			_selectedTower.Deselect();
			_selectedTower = null;
		}
		towerMenu.SetActive(false);
	}
  public bool HasTowerSelected() => _selectedTower != null;

	public void OnTowerBuild(int id)
	{
		var towerRef = towers[id-1];
		var towerData = towerRef.GetComponent<Tower>();
		if (gameState.currentMoney >= towerData.price)
		{
			_selectedTower.BuildTower(towers[id-1]);
			_selectedTower = null;
			towerMenu.SetActive(false);
			DebitMoney(towerData.price);
		}
	}

	public void StartNextWave()
	{
		_currentWave++;
		SpawnManager.Instance.SpawnWave(_currentWave);
	}

	public void WaveFinished()
	{
		if (_currentWave + 1 < SpawnManager.Instance.waves.Count)
			UIManager.Instance.WaveBarStartCountDown(waveDelay);
		else
			GameFinish(false);
	}

	public void GameFinish(bool lose)
	{
		// TODO
		Debug.Log(nameof(GameFinish));
	}

	public uint GetMoney() => gameState.currentMoney;
	public void DebitMoney(uint amount)
	{
		gameState.RemoveMoney(amount, GameState.MoneySpentType.Tower);
		UIManager.Instance.UpdateMoney(gameState.currentMoney);
	}
	public void CreditMoney(uint amount)
	{
		gameState.AddMoney(amount);
		UIManager.Instance.UpdateMoney(gameState.currentMoney);
	}

	public uint GetLife() => gameState.currentLife;
	public void DecreaseLife(uint amount = 1)
	{
		GameManager.Instance.gameState.RemoveLife(amount);
		UIManager.Instance.UpdateLife(gameState.currentLife);
	}
}
