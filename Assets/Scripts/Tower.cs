using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
	public float areaSize = 5f;
	public float shootSpeed = 1f;
	public float damage = 1f;
	public uint price;

	public GameObject cannon;
	public GameObject bulletPrefab;
	public Transform bulletStartPosition;
	public GameObject area;

	List<GameObject> _enemiesInRange = new();
	GameObject _currentTarget;
	float _lastShoot;

	void Start()
	{
		GetComponent<CircleCollider2D>().radius = areaSize/2;
		area.transform.localScale = new Vector3(areaSize, areaSize, areaSize);
	}

	void FixedUpdate()
	{
		if (_enemiesInRange.Count > 0 && _currentTarget == null)
			_currentTarget = _enemiesInRange.FirstOrDefault();
		else if (_currentTarget != null)
		{
			LookToTarget();
			if (Time.time - _lastShoot > 1f / shootSpeed && _currentTarget.GetComponent<Enemy>().IsAlive())
				ShootTarget();
			if (!_currentTarget.GetComponent<Enemy>().IsAlive())
			{
				if (_enemiesInRange.Count > 0)
					_enemiesInRange.RemoveAt(0);
				_currentTarget = null;
			}
		}
	}

	void LookToTarget()
	{
		Vector3 relative = cannon.transform.InverseTransformPoint(_currentTarget.transform.position);
		cannon.transform.Rotate(0, 0, Mathf.Atan2(relative.x, relative.y) * -Mathf.Rad2Deg);
	}

	void ShootTarget()
	{
		_lastShoot = Time.time;
		var bullet = Instantiate(bulletPrefab, cannon.transform.position, Quaternion.identity);
		bullet.transform.position = bulletStartPosition.position;
		bullet.GetComponent<Bullet>().SetBullet(_currentTarget, damage);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		_enemiesInRange.Add(other.gameObject);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Enemy>().IsAlive())
		{
			_enemiesInRange.Remove(other.gameObject);
			if (_currentTarget == other.gameObject)
				_currentTarget = null;
		}
	}
}
