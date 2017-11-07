using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
	public SceneController sceneCtl;

	public ScoreManeger scManeger;

	public Text[] scoreText;
	public Text calcuText;
	public Text yourText;
	public Text rankInText;

	private bool isRetrunStart = false;
	private bool inputAnyKey = false;

	private string score_1 = "score_1";
	private string score_2 = "score_2";
	private string score_3 = "score_3";
	private string score_4 = "score_4";
	private string score_5 = "score_5";

	void Start (){
		StartCoroutine (RetrunStart ());

		CheckRank ();

		DispRank (0, score_1);
		DispRank (1, score_2);
		DispRank (2, score_3);
		DispRank (3, score_4);
		DispRank (4, score_5);

		// 自分がどんなスコアだったかを表示
		calcuText.text = "経過時間:" + scManeger.time + "×" + "エネミー撃破スコア:" + scManeger.score;
		//　自分が獲得したスコアを表示
		yourText.text = "あなたの最終スコアは" + scManeger.yourScore + "でした";
	}

	void Update ()
	{
		// このシーンに来てから２秒後から何かしらの入力があるとタイトルに戻る。
		if (Input.anyKey && isRetrunStart) 
		{
			inputAnyKey = true;
		}
		if (inputAnyKey) 
		{
			sceneCtl.Fead ();
		}
	}

	private IEnumerator RetrunStart()
	{
		yield return new WaitForSeconds (2);
		isRetrunStart = true;
	}

	// 取得したスコアに応じてランキングの変動
	void CheckRank()
	{
		if (scManeger.yourScore >= PlayerPrefs.GetInt (score_1)) 
		{
			PlayerPrefs.SetInt (score_5, PlayerPrefs.GetInt (score_4));
			PlayerPrefs.SetInt (score_4, PlayerPrefs.GetInt (score_3));
			PlayerPrefs.SetInt (score_3, PlayerPrefs.GetInt (score_2));
			PlayerPrefs.SetInt (score_2, PlayerPrefs.GetInt (score_1));
			PlayerPrefs.SetInt (score_1, scManeger.yourScore); 
		} 
		else if (scManeger.yourScore >= PlayerPrefs.GetInt (score_2)) 
		{
			PlayerPrefs.SetInt (score_5, PlayerPrefs.GetInt (score_4));
			PlayerPrefs.SetInt (score_4, PlayerPrefs.GetInt (score_3));
			PlayerPrefs.SetInt (score_3, PlayerPrefs.GetInt (score_2));
			PlayerPrefs.SetInt (score_2, scManeger.yourScore);
		} 
		else if (scManeger.yourScore >= PlayerPrefs.GetInt (score_3)) 
		{
			PlayerPrefs.SetInt (score_5, PlayerPrefs.GetInt (score_4));
			PlayerPrefs.SetInt (score_4, PlayerPrefs.GetInt (score_3));
			PlayerPrefs.SetInt (score_3, scManeger.yourScore);
		} 
		else if (scManeger.yourScore >= PlayerPrefs.GetInt (score_4)) 
		{
			PlayerPrefs.SetInt (score_5, PlayerPrefs.GetInt (score_4));
			PlayerPrefs.SetInt (score_4, scManeger.yourScore);
		}
		else if(scManeger.yourScore >= PlayerPrefs.GetInt (score_5))
		{
			PlayerPrefs.SetInt (score_5,scManeger.yourScore);
		}
	}

	// ランクに応じたスコアを表示
	void DispRank(int rankNum , string rankKey)
	{
		scoreText [rankNum].text = (rankNum + 1).ToString () + "　" + PlayerPrefs.GetInt(rankKey);
		// ランクインしたら色を変える
		if (PlayerPrefs.GetInt(rankKey) == scManeger.yourScore) 
		{
			scoreText [rankNum].color = new Color (1, 0, 0);
			rankInText.gameObject.SetActive (true);
		}
	}
}
