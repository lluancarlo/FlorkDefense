using UnityEngine;
using UnityEngine.Events;

public class FollowPath : MonoBehaviour
{
	public UnityEvent<GameObject> reachFinalPathEvent;
	public SpriteRenderer mesh;
	public float speed = 2f;

	Rigidbody2D _rb;
	Transform _target;
	Enemy _enemy;
	uint _pathIndex = 0;

	void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		_enemy = GetComponent<Enemy>();
		_target = MapManager.Instance.path[_pathIndex];
	}

	void Update()
	{
		if (!_enemy.IsAlive())
			return;
		if (Vector2.Distance(_target.position, transform.position) < 0.1f)
		{
			_pathIndex++;
			if (_pathIndex < MapManager.Instance.path.Length)
				_target = MapManager.Instance.path[_pathIndex];
			else
				reachFinalPathEvent.Invoke(gameObject);
		}
	}

	void FixedUpdate()
	{
		if (!_enemy.IsAlive()){
			_rb.velocity = Vector2.zero;
			return;
		}
		_rb.velocity = (_target.position - transform.position).normalized * speed;
		if (_rb.velocity.x < 0f && !mesh.flipX)
			mesh.flipX = true;
		else if (_rb.velocity.x > 0f && mesh.flipX)
			mesh.flipX = false;
	}
}
