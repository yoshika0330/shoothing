using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	BeamController beamCtrl;

	public PlayerManager plManeger;

	private bool isShoot = true;
	private bool isHitCheck = true;
	private bool isHitDamage = true;
	private bool hitCheckStop = false;
	private bool atackStop = false;
	private bool stopMove = false;

	private float RotateGunpointZ = 0;
	private float calcRotateZ = 1;

	private int checkHp;

	void Start () 
	{
		GameController.Instance.playerHp = plManeger.hp;
		checkHp = plManeger.hp;
		beamCtrl = plManeger.beamImg[0].GetComponent<BeamController> ();
	}

	void Update () {
		if (GameController.Instance.gameState ==
			GameController.GameState.Play) 
		{
			PlayerMove ();
			StartCoroutine (HitDamage ());
			if (atackStop == false) 
			{
				PlayerAtack ();
			}
			if (hitCheckStop == false) 
			{
				StartCoroutine (HitCheck ());
			}
		}
	}

	void PlayerMove()
	{
		//	現在位置を代入
		Vector2 Position = transform.position;

		//	移動できる範囲を制限
		Position = new Vector2 (Mathf.Clamp (transform.position.x, GameController.Instance.dispLeft, GameController.Instance.dispRight),
			Mathf.Clamp(transform.position.y,GameController.Instance.dispBottom,GameController.Instance.dispTop));

		if(stopMove == false)
		{
			//---------------------------------------------
			//	左右の動きの処理
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				Position.x += plManeger.moveSpeed.x;
			} 
			else if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
			{
				Position.x -= plManeger.moveSpeed.x;
			}
			//---------------------------------------------

			//---------------------------------------------
			//	上下の動きの処理
			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) 
			{
				Position.y += plManeger.moveSpeed.y; 
			}
			else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) 
			{
				Position.y -= plManeger.moveSpeed.y; 
			}
			//---------------------------------------------
		}
		//	結果を反映
		transform.position = Position;

		//	銃口を常に左右に振り続ける処理
		//-------------------------------
		RotateGunpointZ += calcRotateZ;

		if (RotateGunpointZ >= plManeger.MaxGunPoint) {
			calcRotateZ *= -1;		
		}
		else if (RotateGunpointZ <= plManeger.MaxGunPoint * -1) 
		{
			calcRotateZ *= -1;
		}
		//-------------------------------

		//	体力がなくなったらゲームのステートをエンドにする
		if (GameController.Instance.playerHp <= 0) 
		{
			GameController.Instance.playerHp = 0;
			GameController.Instance.gameState = GameController.GameState.End;
		}
	}

	void PlayerAtack()
	{
		StartCoroutine (atackBeam ());

		//	スキルの発動
		// 発動条件はダメージを食らってる最中ではないときにゲージが満タンでスペースキーを押したとき
		if (Input.GetKeyDown (KeyCode.Space) && 
			GameController.Instance.skilBar.value == 1
			&& GameController.Instance.isPlayerHit == false) 
		{
			GameController.Instance.isChargeGauge = false;
			// スキルゲージの初期化
			GameController.Instance.skilBar.value = 0;
			// スキルレーザーを出す
			Instantiate (plManeger.beamImg [1], transform.position, transform.rotation);
			// スキルSEの再生
			SoundController.Instance.PlaySkillSE();
			// スキル発動中は無敵にする
			this.GetComponent<BoxCollider2D>().enabled = false;
			StartCoroutine (CoolTime ());
		}
	}

	void GetItem()
	{
		stopMove = true;
		StartCoroutine (StartMove ());
	}
		
	//　弾を出す処理
	private IEnumerator atackBeam()
	{
		if (isShoot) 
		{
			isShoot = false;
			beamCtrl.direction = RotateGunpointZ;
			//	弾の画像を出す
			Instantiate (plManeger.beamImg [0],transform.position,Quaternion.Euler(0,0,beamCtrl.direction));
			// 弾の発射SE再生
			SoundController.Instance.PlayShotSE ();
			//	どのくらいの間隔で弾を発射するか
			yield return new WaitForSeconds (plManeger.waitShootTime);
			isShoot = true;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		// エネミーか地雷に当たった時にダメージを減らす処理
		// 反射したビームに当たった時のダメージ処理はビームのほうに記述
		if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Bom" || col.gameObject.tag == "BossEnemy") {
			GameController.Instance.playerHp--;
			GameController.Instance.isPlayerHit = true;
		}
		else if (col.gameObject.tag == "Item") 
		{
			GetItem ();
		}
	}

	// ノーダメージ判定
	private IEnumerator HitCheck()
	{
		if (isHitCheck) 
		{
			isHitCheck = false;
			yield return new WaitForSeconds (plManeger.checkDamageTime);
			if (hitCheckStop == false)
			{
				if (checkHp == GameController.Instance.playerHp)
				{
					GameController.Instance.isChargeGauge = true;
				}
				else
				{
					GameController.Instance.isChargeGauge = false;
					checkHp = GameController.Instance.playerHp;
				}
			}
			isHitCheck = true;
		}
	}

	private IEnumerator CoolTime()
	{
		hitCheckStop = true;
		atackStop = true;
		yield return new WaitForSeconds (plManeger.coolTime);
		// 無敵の解除
		this.GetComponent<BoxCollider2D> ().enabled = true;
		hitCheckStop = false;
		atackStop = false;
	}

	// ダメージを受けた時の点滅の処理
	private IEnumerator HitDamage()
	{
		int flashingCount = 0;
		if (isHitDamage && GameController.Instance.isPlayerHit) 
		{
			SoundController.Instance.PlayPlayerHitSE ();
			isHitDamage = false;
			//　点滅中はあたり判定を消す
			this.GetComponent<BoxCollider2D> ().enabled = false;
			while (flashingCount < 5) 
			{
				GetComponent<SpriteRenderer> ().enabled = false;
				yield return new WaitForSeconds (0.2f);
				GetComponent<SpriteRenderer> ().enabled = true;
				flashingCount++;
				yield return null;
			}
			isHitDamage = true;
			GameController.Instance.isPlayerHit = false;
			//　点滅が終わったら少し経ったらあたり判定を出す
			yield return new WaitForSeconds (0.2f);
			this.GetComponent<BoxCollider2D> ().enabled = true;
		}
	}
	private IEnumerator StartMove()
	{
		// マネージャーで設定した時間止まる
		yield return new WaitForSeconds (plManeger.stopTime);
		stopMove = false;
	}
}