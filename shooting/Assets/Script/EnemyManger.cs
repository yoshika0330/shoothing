using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyManger : ScriptableObject 
{

	// 動く速さ
	public Vector2 moveSpeed;
	// 何秒おきに攻撃してくるか
	public float waitShootTime;
	//	体力
	public int hp;
	// 倒した時の得点
	public int score;
}
