using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScoreManeger :ScriptableObject 
{
	// 過去のスコアを保存
	public List<int> totalScore = new List<int> ();
	// 計算したスコア
	[HideInInspector]
	public int yourScore;
	// 耐えきった時間
	[HideInInspector]
	public int time;
	// 倒した敵のスコア
	[HideInInspector]
	public int score;
}
