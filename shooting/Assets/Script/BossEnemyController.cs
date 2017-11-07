using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyController : MonoBehaviour {

	//　ボスのステータスレベル
	public BossEnemyManeger[] bossManeger;

	public GameObject bossEnemyScore;

	private bool setRandomNum = true;
	private bool isHitDamage = true;
	private bool isMove = false;
	private bool isFirstMove = true;

	private Vector3 randomMoveSpeed;

	private Vector2 marginalRange;
	private Vector2 randomMoveSpeedRange;

	private int manegerNum;
	private int hp;
	private int score;

	void Start () 
	{
		manegerNum = GameController.Instance.bossManegerNum;
		marginalRange = bossManeger [manegerNum].marginalRange;
		randomMoveSpeedRange = bossManeger [manegerNum].randomMoveSpeed;
		hp = bossManeger [manegerNum].hp;
		score = bossManeger [manegerNum].score;
		// 次にボスが出てきたときに用に用意していたステータスレベルを上げる
		if (GameController.Instance.bossManegerNum < bossManeger.Length - 1) 
		{
			GameController.Instance.bossManegerNum++;
		}

		GameController.Instance.bossHpBar.maxValue = hp;
		GameController.Instance.bossHpBar.value = hp;
	}

	void Update () 
	{
		if (isFirstMove) 
		{
			FirstMove ();
		}

		if (GameController.Instance.gameState == 
			GameController.GameState.Play && isMove) 
		{
			Move ();
		}
	}

	private void Move()
	{
		StartCoroutine (RandomMoveSpeedSet ());
		// 動きの制限を設ける
		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x, marginalRange.x * -1 , marginalRange.x),
			Mathf.Clamp(transform.position.y,marginalRange.y * -1,marginalRange.y),
			0);
		//　移動させる
		transform.position += randomMoveSpeed;
	}

	// 最初の画面内に入ってくる動き
	private void FirstMove()
	{
		if (transform.position.y > marginalRange.y) 
		{
			transform.position -= new Vector3 (0, 0.02f, 0);
			if(transform.position.y <= marginalRange.y)
			{
				isFirstMove = false;
				isMove = true;
				GetComponent<BoxCollider2D> ().enabled = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.gameObject.tag == "SkilBeam") 
		{
			StartCoroutine (Flashing ());
			StartCoroutine (HitDamage ());
		}
	}

	private IEnumerator RandomMoveSpeedSet()
	{
		if (setRandomNum) 
		{
			setRandomNum = false;
			// ランダムで動くスピードを0.5秒おきに決める
			randomMoveSpeed.x = Random.Range (randomMoveSpeedRange.x * -1, randomMoveSpeedRange.x);
			randomMoveSpeed.y = Random.Range (randomMoveSpeedRange.y * -1, randomMoveSpeedRange.y);
			randomMoveSpeed.z = 0;
			yield return new WaitForSeconds (0.5f);
			setRandomNum = true;
		}
	}

	// ダメージの処理（0.1秒ごと）
	private IEnumerator HitDamage()
	{
		if (isHitDamage) 
		{
			isHitDamage = false;
			hp--;
			GameController.Instance.bossHpBar.value = hp;
			if (hp <= 0) 
			{
				SoundController.Instance.PlayEnemyHitSE ();
				// スコアも動いてしまうから動きを止める
				isMove = false;
				//　画像やあたり判定を無くす
				GetComponent<SpriteRenderer> ().enabled = false;
				GetComponent<BoxCollider2D> ().enabled = false;
				GameController.Instance.bossHpBar.gameObject.SetActive (false);
				//　スコアを加算して表示させる
				GameController.Instance.score += score;
				GameController.Instance.scoreText.text = "Score" + GameController.Instance.score;
				//　倒された位置に自身のスコアを表示する
				bossEnemyScore.GetComponent<TextMesh> ().text = "+" + score;
				bossEnemyScore.SetActive (true);
				//　ディレイをかけて消す
				StartCoroutine (DeleteObj ());
			}
			yield return new WaitForSeconds (0.1f);
			isHitDamage = true;
		}
	}

	// ダメージを受けてる時の点滅の処理
	private IEnumerator Flashing()
	{

		GetComponent<SpriteRenderer> ().enabled = false;
		yield return new WaitForSeconds (0.2f);
		if (hp > 0) 
		{
			GetComponent<SpriteRenderer> ().enabled = true;
		}
	}

	//　消す処理
	private IEnumerator DeleteObj()
	{
		yield return new WaitForSeconds (1f);
		GameController.Instance.isBossPop = true;
		Destroy (gameObject);
	}
}
