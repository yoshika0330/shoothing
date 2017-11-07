using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerManager : ScriptableObject 
{

	//	プレイヤーの移動速度
	public Vector2 moveSpeed;
	//	プレイヤーのビームの画像
	public GameObject[] beamImg;
	//	ビームの発射間隔
	public float waitShootTime;
	// 体力
	public int hp;
	//	銃口の傾きの限界
	public int MaxGunPoint;
	// 何秒おきにダメージ判定をとるか
	public float checkDamageTime;
	// クールタイムの設定
	public float coolTime;
	// アイテムを拾ったときに止まる時間
	public float stopTime;
}
