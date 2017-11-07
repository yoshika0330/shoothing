using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public EnemyManger eneManeger;

	public GameObject enemyScore;

	private bool onCamera = false;
	private bool isMove = true;

	private int hp;
	private int moveNo = 0;
	private int i = 0;

	private Vector2 moveSpeed;

	void Start () 
	{
		hp = eneManeger.hp;
		moveSpeed = eneManeger.moveSpeed;
		// 一定時間経過したら動きにばらつきを出す
		if(GameController.Instance.time >= 
			GameController.Instance.gmManeger.enemyMoveChangeStartTime)
		{
			moveNo = Random.Range (0, 4);
		}
	}

	void Update () {

		if (GameController.Instance.gameState == 
			GameController.GameState.Play && isMove) 
		{
			Move ();
		}
	}

	private void Move()
	{
		//	ランダムで決められた動きをする
		switch(moveNo)
		{
		case 0:
			transform.position -= new Vector3 (0, moveSpeed.y, 0);
			break;
		case 1:
			transform.position -= new Vector3 (moveSpeed.x , moveSpeed.y, 0);
			break;
		case 2:
			transform.position -= new Vector3 (moveSpeed.x * -1 , moveSpeed.y, 0);
			break;
		case 3:
			transform.position -= new Vector3 (moveSpeed.x * 2, moveSpeed.y, 0);
			i++;
			if (i >= 120) 
			{
				i = 0;
				moveSpeed.x *= -1;
			}
			break;
		default:
			break;
		}
		//	下に見切れたら消す
		if (transform.position.y <= GameController.Instance.dispBottom) 
		{
			Destroy (gameObject);
		}
		//	画面に映っていたら当たり判定をつける
		if (onCamera) 
		{
			this.GetComponent<BoxCollider2D> ().enabled = true;
		}
		//　映っていないときは当たり判定を消す
		else if (onCamera == false) 
		{
			this.GetComponent<BoxCollider2D> ().enabled = false;
		}

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//	ビームに当たって体力がなくなったら消す
		if (col.gameObject.tag == "Beam") 
		{
			hp--;
			ShotDown ();
		} 
		else if (col.gameObject.tag == "SkilBeam")
		{
			hp = 0;
			ShotDown ();
		}
		else if (col.gameObject.tag == "Player")
		{
			Destroy (gameObject);
		}
	}

	//	画面に映っているか判定
	//------------------------------
	void OnBecameInvisible()
	{
		onCamera = false;
	}

	void OnBecameVisible()
	{
		onCamera = true;
	}
	//------------------------------

	void ShotDown()
	{
		if (hp <= 0)
		{
			// スコアも動いてしまうから動きを止める
			isMove = false;
			//　画像やあたり判定を無くす
			GetComponent<SpriteRenderer> ().enabled = false;
			GetComponent<BoxCollider2D> ().enabled = false;
			// 倒された時にSEを再生
			SoundController.Instance.PlayEnemyHitSE ();
			//　スコアを加算して表示させる
			GameController.Instance.score += eneManeger.score;
			GameController.Instance.scoreText.text = "Score:" + GameController.Instance.score;
			//　倒された位置に自身のスコアを表示する
			enemyScore.GetComponent<TextMesh> ().text = "+" + eneManeger.score;
			enemyScore.SetActive (true);
			//　ディレイをかけて消す
			StartCoroutine (DeleteObj ());
		}
	}

	private IEnumerator DeleteObj()
	{
		yield return new WaitForSeconds (0.5f);
		Destroy (gameObject);
	}
}
