using TMPro;
using UnityEngine;

public class TowerSlot : MonoBehaviour, Selectable
{
	public SpriteRenderer mesh;
	public bool isSelected;

	void Awake() => ValidadeProperties();

	void ValidadeProperties()
	{
		if (mesh == null)
			throw new GameExceptions.MissingGameObjectReference(name, typeof(SpriteRenderer));
	}

	private void Start()
	{
		Deselect();
	}

	public void Select()
	{
		isSelected = true;
		mesh.color = new Color(1f, 1f, 1f, 1f);
	}

	public void Deselect()
	{
		isSelected = false;
		mesh.color = new Color(1f, 1f, 1f, 0.25f);
	}

	public void BuildTower(GameObject tower)
	{
		Instantiate(tower, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
