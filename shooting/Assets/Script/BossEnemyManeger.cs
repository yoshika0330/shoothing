using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossEnemyManeger : ScriptableObject 
{

	// ボスの体力
	public int hp;
	// ボスの移動範囲制限
	public Vector2 marginalRange;
	// ボスの移動のランダムのスピードの範囲
	public Vector2 randomMoveSpeed;
	// ボスのスコア
	public int score;

}
