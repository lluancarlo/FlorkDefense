using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;
	public Transform spawnPosition;
	public float spawnTime = 0.5f;
	public GameObject[] enemiesPrefab;
	public List<GameObject> currentWave;
	public List<Wave> waves;
	int enemiesAliveCurrentWave = 0;

	void Awake()
	{
		if (Instance != null)
			return;
		Instance = this;
		ValidadeProperties();
		CreateWaves();
	}

	void ValidadeProperties()
	{
		if (enemiesPrefab.Length == 0)
			throw new GameExceptions.MissingGameObjectReference(name, typeof(GameObject[]));
	}

	void CreateWaves()
	{
		waves = new List<Wave>();
		for (int i = 0; i < enemiesPrefab.Length; i++)
			foreach (var n in new[] { 5, 10, 15 })
			{
				waves.Add(new Wave
				{
					enemyPrefab = enemiesPrefab[i],
					quantity = (uint)n
				});
			}
	}

	public void SpawnWave(int waveId)
	{
		StartCoroutine(SpawnWave(waves[waveId]));
	}

	IEnumerator SpawnWave(Wave wave)
	{
		currentWave.Clear();
		var enemiesSpawned = 0;
		while (enemiesSpawned < wave.quantity)
		{
			var enemy = Instantiate(wave.enemyPrefab, spawnPosition);
			enemy.GetComponent<FollowPath>().reachFinalPathEvent.AddListener(OnFinalReached);
			enemy.GetComponent<Enemy>().deadEvent.AddListener(OnEnemyDie);
			enemy.transform.parent = gameObject.transform;
			currentWave.Add(enemy);
			enemiesSpawned++;
			enemiesAliveCurrentWave++;
			yield return new WaitForSeconds(spawnTime);
		}
	}

	void OnFinalReached(GameObject enemy)
	{
		Destroy(enemy);
		enemiesAliveCurrentWave--;
		GameManager.Instance.DecreaseLife();
		if (GameManager.Instance.GetLife() == 0)
			GameManager.Instance.GameFinish(true);
	}

	void OnEnemyDie(GameObject enemy)
	{ 
		var enemyData = enemy.GetComponent<Enemy>();
		GameManager.Instance.CreditMoney(enemyData.value);
		Destroy(enemy);
		enemiesAliveCurrentWave--;
		if (enemiesAliveCurrentWave == 0)
			GameManager.Instance.WaveFinished();
	}
}
