using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedImage : MonoBehaviour
{
	public Image image;
	public Sprite[] sprites;
	public float speed = .02f;

	int _index;

	void Start()
	{
		StartCoroutine(PlayAnim());
	}

	IEnumerator PlayAnim()
	{
		while (true)
		{
			yield return new WaitForSeconds(speed);
			if (_index >= sprites.Length)
				_index = 0;
			image.sprite = sprites[_index];
			_index += 1;
		}
	}
}