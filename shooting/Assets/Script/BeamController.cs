using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour {

	public float beamDestroyTime = 10f;
	[HideInInspector]
	public float direction;

	public Vector2 moveSpeed = new Vector2(5,5);

	private Vector2 movePoint;

	private Rigidbody2D rd;

	private bool isOnceRebound = false;

	void Start () 
	{
		rd = GetComponent<Rigidbody2D> ();
	}


	void Update () 
	{
		if (GameController.Instance.gameState == 
			GameController.GameState.Play)
		{
			move ();
			StartCoroutine (DeleteObj ());
		}
		else if (GameController.Instance.gameState == 
			GameController.GameState.End) 
		{
			rd.simulated = false;
		}
	}

	//	ビームの移動の制御
	void move()
	{
		//-------------------------------------------------------------------------------------
		// 反射の処理
		// 横の反射
		if (GameController.Instance.dispLeft >= transform.position.x ||
			GameController.Instance.dispRight<= transform.position.x) 
		{
			moveSpeed.x *= -1;
			OnceRebound ();
		}
		// 縦の反射
		if (GameController.Instance.dispBottom >= transform.position.y ||
			GameController.Instance.dispTop<= transform.position.y) 
		{
			moveSpeed.y *= -1;
			OnceRebound ();
		}
		//-------------------------------------------------------------------------------------

		//-------------------------------------------------------------------------------------
		// 動きの処理
		movePoint.x = Mathf.Cos(Mathf.Deg2Rad * (direction + 90) ) * moveSpeed.x;
		movePoint.y = Mathf.Sin (Mathf.Deg2Rad * (direction + 90) ) * moveSpeed.y;

		if (direction == 0) 
		{
			movePoint.x = 0;
		}
		rd.velocity = movePoint;
		//-------------------------------------------------------------------------------------
	}
		
	// 初回の跳ね返った処理
	void OnceRebound()
	{
		if (isOnceRebound == false) 
		{
			GetComponent<SpriteRenderer> ().enabled = false;
			isOnceRebound = true;
		}
	}

	IEnumerator DeleteObj()
	{
		//	一定時間後ビームを削除
		yield return new WaitForSeconds (beamDestroyTime);
		if (GameController.Instance.gameState ==
			GameController.GameState.Play) 
		{
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//	ビームが相手に当たったら消す
		if (col.gameObject.tag == "Enemy")
		{
			Destroy (gameObject);
		}
		if (col.gameObject.tag == "Player") 
		{
			//	一回壁に当たったら自分に当たるようにする
			if (isOnceRebound) 
			{
				GameController.Instance.isPlayerHit = true;
				GameController.Instance.playerHp--;
				Destroy (gameObject);
			}
		}
	}
}