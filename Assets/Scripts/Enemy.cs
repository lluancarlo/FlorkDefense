using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	public Animator animControl;
	public Slider healthBar;
  public float damageTaken { get; private set; } = 0f;
	public UnityEvent<GameObject> deadEvent;
	
	public float totalLife = 100f;
	public uint value = 1;
	int _animControlParamIsDead = Animator.StringToHash("isDead");

	public void AddDamage(float damage)
	{
		if (!IsAlive())
			return;
		damageTaken += damage;
		healthBar.value = (totalLife - damageTaken) / totalLife;
		if (!IsAlive())
			Die();
	}

	void Die(){
		healthBar.gameObject.SetActive(false);
		animControl.SetBool(_animControlParamIsDead, true);
		StartCoroutine(WaitForAnimationFinished());
	}

	IEnumerator WaitForAnimationFinished()
	{
		yield return new WaitForSeconds(1f);
		deadEvent.Invoke(gameObject);
	} 

	public bool IsAlive() => totalLife - damageTaken > 0f;
}
