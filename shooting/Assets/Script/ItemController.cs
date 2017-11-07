using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

	public float speed;

	void Update () 
	{
		if (GameController.Instance.gameState == GameController.GameState.Play) 
		{
			Move ();
		}
	}

	void Move()
	{
		transform.position -= new Vector3 (0, speed, 0);
		if (transform.position.y < GameController.Instance.dispBottom) 
		{
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			SoundController.Instance.PlayGetItemSE ();
			Destroy (gameObject);
		}
	}
}
