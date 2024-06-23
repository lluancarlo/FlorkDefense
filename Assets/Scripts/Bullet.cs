using UnityEngine;

public class Bullet : MonoBehaviour
{
  public float speed = 5f;

  float _damage = 1f;
  GameObject _target;
  Rigidbody2D _rb;
  Vector3 _targetOffset = new Vector3(0f, 0.25f, 0f);

	void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
  }

  void FixedUpdate()
  {
    _rb.velocity = (_target.transform.position + _targetOffset - transform.position).normalized * speed;
    if (Vector3.SqrMagnitude(transform.position - _target.transform.position) < 0.1f)
    {
      _target.GetComponent<Enemy>().AddDamage(_damage);
      Destroy(gameObject);
    }
    else if (!_target.GetComponent<Enemy>().IsAlive())
      Destroy(gameObject);
  }

  public void SetBullet(GameObject target, float damage)
  {
    this._target = target;
		this._damage = damage;
  }
}
