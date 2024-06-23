using UnityEngine;

public class MapManager : MonoBehaviour
{
	public static MapManager Instance;
	public Transform[] path;

	void Awake()
	{
		if (Instance != null)
			return;
		Instance = this;
	}
}
