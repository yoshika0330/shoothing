using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameManeger : ScriptableObject 
{

	// 敵の出現頻度
	public float enemyPopSpeed;
	// 地雷の出現頻度
	public float bomPopSpeed;
	// ゲーム開始後何秒後に地雷の出現を開始するか
	public float bomPopStartTime;
	// 地雷の出現頻度を上げるのを何秒おきにするか
	public float bomPopSpeedUpTime;
	// ゲージのチャージ速度
	public float chargeSpeed;
	// 敵の動きのルーチンを変える開始の時間
	public float enemyMoveChangeStartTime;
	// ボスの出現を何秒おきにするか
	public float bossPopTime;
	// ボスをゲーム開始から何秒後に出現させるか
	public float bossPopStartTime;
	// アイテムを何秒おきに生成しようとするか
	public float itemPopSpeed;
	// アイテムをどれくらいの確率で出すか
	public int itemPopPercent;


}
