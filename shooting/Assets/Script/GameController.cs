using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public ScoreManeger scManeger;
	public GameManeger gmManeger;
	public SceneController sceneCtrl;

	Camera mainCamera;

	// 敵の画像
	public GameObject[] enemyObj;
	// ボスの画像
	public GameObject bossEnemyObj;
	// ボムの画像
	public GameObject bomObj;
	// アイテムの画像
	public GameObject itemObj;
	public GameObject skilBarColor;

	public Text scoreText;
	public Text timeText;
	public Text centerText;
	public Text tutorialText;

	public Button NextButton;
	public Button BackButton;
	public Button SkipButton;

	public string[] tutorial;

	[HideInInspector]
	public int playerHp;
	[HideInInspector]
	public int score;
	[HideInInspector]
	public int bossManegerNum = 0;

	[HideInInspector]
	public float time;
	[HideInInspector]
	public float dispLeft, dispRight, dispTop, dispBottom;

	[HideInInspector]
	public bool isChargeGauge = false;
	[HideInInspector]
	public bool isPlayerHit = false;
	[HideInInspector]
	public bool isBossPop = true;

	private float enemyPopSpeed;
	private float bomPopSpeed;

	[HideInInspector]
	public Slider skilBar;
	public Slider bossHpBar;

	private bool isPopEnemy = true;
	private bool isPopBom = true;
	private bool isPopItem = true;
	private bool isCountDown = true;
	private bool isAddScore = true;
	private bool tempoUp = true;
	private bool playOnceMainBGM = true;
	private bool playOnceGameOverBGM = true;
	private bool playOnceSkillChargeSe = true;

	private Vector2 enemyRandomPos;
	private Vector2 bomRandomPos;

	private Slider hpBar;

	private int totalScore;
	[HideInInspector]
	public int textNum = 0;

	[HideInInspector]
	public enum GameState
	{
		Tutorial,
		Start,
		Play,
		End,
		Result
	};
	[HideInInspector]
	public GameState gameState;

	public static GameController Instance;
	void Awake()
	{
		if (null != Instance) {
			Destroy (gameObject);
		}
		else
		{
			Instance = this;
		}

		//DontDestroyOnLoad (gameObject);
	}

	void Start () {
		mainCamera = Camera.main.GetComponent<Camera> ();

		//---------------------------------------------------------------------------------------------------
		// 画面の左下の座標を取得
		Vector3 bottomLeft = mainCamera.ScreenToWorldPoint (Vector3.zero);
		// 画面の右上の座標を取得
		Vector3 topRight = mainCamera.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0.0f));
		//-------------------------
		//それぞれをわかりやすい変数に入れる
		dispTop = topRight.y;
		dispBottom = bottomLeft.y;
		dispRight = topRight.x;
		dispLeft = bottomLeft.x;
		//---------------------------------------------------------------------------------------------------

		// Hpバーを取得
		hpBar = GameObject.Find ("HPBar").GetComponent<Slider> ();
		// Skilバーを取得
		skilBar = GameObject.Find ("SkilBar").GetComponent<Slider> ();

		// 体力バーの最大値をプレイヤーの体力の最大値と同じ値にする
		hpBar.maxValue = playerHp;
		hpBar.value = playerHp;
		// 初期化
		skilBar.value = 0;

		// タイムとスコアの表示
		timeText.text = "Time:" + 0;
		scoreText.text = "Score:" + 0;

		//　チュートリアルの表示
		tutorialText.text = tutorial [0];

		enemyPopSpeed = gmManeger.enemyPopSpeed;
		bomPopSpeed = gmManeger.bomPopSpeed;
	}

	void Update () {

		// ゲームの状態によってそれぞれの処理を行う
		switch(gameState)
		{
		case GameState.Tutorial:
			TutotialTextCtrl ();
			break;
		case GameState.Start:
			NextButton.gameObject.SetActive (false);
			BackButton.gameObject.SetActive (false);
			SkipButton.gameObject.SetActive (false);
			StartCoroutine (StartCount ());
			break;
		case GameState.Play:
			if (playOnceMainBGM) 
			{
				playOnceMainBGM = false;
				SoundController.Instance.PlayMainBGM ();
			}
			StartCoroutine (PopEnemy ());
			StartCoroutine (GameTempoUp ());
			StartCoroutine (PopItem ());
			// ゲームがスタートしてから何秒後から何秒おきにボスを一体出すか
			if ((int)time >= gmManeger.bossPopStartTime &&(int)time % gmManeger.bossPopTime == 0 && isBossPop) 
			{
				isBossPop = false;
				Instantiate (bossEnemyObj);
				bossHpBar.gameObject.SetActive (true);
			}
			// ゲームがスタートして何秒後に地雷が生成されるか
			if (time >= gmManeger.bomPopStartTime) 
			{
				StartCoroutine (PopBom ());
			}
			GetTime ();
			ChargeGauge ();
			hpBar.value = playerHp;
			break;
		case GameState.End:
			if (playOnceGameOverBGM) 
			{
				playOnceGameOverBGM = false;
				SoundController.Instance.PlayGameOverBGM ();
			}
			hpBar.value = playerHp;
			centerText.gameObject.SetActive (true);
			centerText.text = "GameOver";
			AddScore ();
			StartCoroutine (StartFead ());
			break;
		default:
			break;
		}
	}

	// チュートリアルのテキストの管理
	private void TutotialTextCtrl()
	{
		// スペースが押されたらスキップ
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			EndText ();
		}

		// 右キーが入力されたらテキストを次に進める
		else if (Input.GetKeyDown (KeyCode.RightArrow))
		{
			NextText ();
		}
		// 左キーが入力されたらテキストを戻す
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			BackText ();
		}
		if(tutorial.Length == textNum)
		{
			// 最後まで行って何か押されたらゲーム開始
			EndText();
		}

		if (textNum == 0) 
		{
			BackButton.gameObject.SetActive (false);
		}
		else
		{
			BackButton.gameObject.SetActive (true);
		}
	}

	public void NextText()
	{
		SoundController.Instance.PlayTextSE ();
		textNum++;
		if (textNum < tutorial.Length)
		{
			tutorialText.text = tutorial [textNum];
		}
	}

	public void BackText()
	{
		if (textNum != 0) 
		{
			SoundController.Instance.PlayTextSE ();
			textNum--;
			tutorialText.text = tutorial [textNum];
		}
	}

	public void EndText()
	{
		gameState = GameState.Start;
		GameObject.Find ("TutorialText").SetActive (false);
	}

	private void GetTime()
	{
		//	時間を取得
		time += Time.deltaTime;
		//　時間を表示
		timeText.text = "Time:" + (int)time;
	}

	private void ChargeGauge()
	{
		// ゲージを溜める処理
		if (isChargeGauge) 
		{
			skilBar.value += Time.deltaTime * gmManeger.chargeSpeed;
		}

		//--------------------------------------------------------------------
		// ゲージの色を変える処理
		if (skilBar.value >= 1) 
		{
			if (playOnceSkillChargeSe) 
			{
				playOnceSkillChargeSe = false;
				SoundController.Instance.PlaySkillChargeSE ();
			}
			skilBarColor.GetComponent<Image> ().color = new Color (1, 1, 0);
		}
		else 
		{
			playOnceSkillChargeSe = true;
			skilBarColor.GetComponent<Image> ().color = new Color (1, 1, 1);
		}
		//--------------------------------------------------------------------
	}

	private void AddScore()
	{
		if (isAddScore) 
		{
			isAddScore = false;
			// リザルトに時間を表示させる用
			scManeger.time = (int)time;
			// リザルトに撃破スコアを表示させる用
			scManeger.score = score;
			//　トータルスコアの計算
			totalScore = (int)time * score;
			//　リザルトにトータルスコアを表示させる用
			scManeger.yourScore = totalScore;
			//　今までのスコアリストに追加
			scManeger.totalScore.Add (totalScore);
			//　リストのソート（降順）
			scManeger.totalScore.Sort ((a, b) => b - a);
		}
	}

	//	エネミーをポップさせる処理
	private IEnumerator PopEnemy()
	{
		//　カメラが見切れる範囲内からランダムの値を摘出
		enemyRandomPos.x = Random.Range (dispLeft, dispRight);
		if (isPopEnemy)
		{
			isPopEnemy = false;
			Instantiate (enemyObj [Random.Range(0,enemyObj.Length)], new Vector3 (enemyRandomPos.x, dispTop + 1, 0), transform.rotation = new Quaternion(0,0,180,0));
			yield return new WaitForSeconds (enemyPopSpeed);
			isPopEnemy = true;
		}
	}

	// アイテムをポップさせる処理
	private IEnumerator PopItem()
	{
		//　カメラが見切れる範囲内からランダムの値を摘出
		float ItemRandomPos = Random.Range (dispLeft, dispRight);
		if (isPopItem)
		{
			isPopItem = false;
			// マネージャーで設定した時間ごとにマネージャーで設定した確率でアイテムを生成
			if (gmManeger.itemPopPercent >= Random.Range (1, 101)) 
			{
				Instantiate (itemObj, new Vector3 (ItemRandomPos, dispTop + 1, 0), transform.rotation);
			}
			yield return new WaitForSeconds (gmManeger.itemPopSpeed);
			isPopItem = true;
		}
	}

	// 地雷を出現させる処理
	private IEnumerator PopBom()
	{
		// 画面内のランダムな値を取得
		bomRandomPos.x = Random.Range (dispLeft, dispRight);
		bomRandomPos.y = Random.Range (dispBottom, dispTop);

		if (isPopBom) 
		{
			isPopBom = false;
			Instantiate (bomObj, new Vector3 (bomRandomPos.x, bomRandomPos.y, 0), transform.rotation);
			//　何秒おきに出現させるか
			yield return new WaitForSeconds (gmManeger.bomPopSpeed);
			isPopBom = true;
		}
	}

	// ゲームの難易度を徐々に上げる
	private IEnumerator GameTempoUp()
	{
		if (tempoUp) 
		{
			tempoUp = false;
			yield return new WaitForSeconds (1);
			// 地雷の出現頻度を上げる
			if ((int)time >= gmManeger.bomPopStartTime && (int)time % gmManeger.bomPopSpeedUpTime == 0 && bomPopSpeed >= 0.2f) 
			{
				bomPopSpeed -= 0.05f;
			}
			tempoUp = true;
		}
	}
		
	// ゲーム開始時のカウントダウン
	private IEnumerator StartCount()
	{
		if (isCountDown) 
		{
			isCountDown = false;
			centerText.gameObject.SetActive (true);
			centerText.text = "3";
			SoundController.Instance.PlayCountSE ();
			yield return new WaitForSeconds (1);
			centerText.text = "2";
			SoundController.Instance.PlayCountSE ();
			yield return new WaitForSeconds (1);
			centerText.text = "1";
			SoundController.Instance.PlayCountSE ();
			yield return new WaitForSeconds (1);
			centerText.text = "GO!";
			SoundController.Instance.PlayCountGoSE ();
			gameState = GameState.Play;
			yield return new WaitForSeconds (0.5f);
			centerText.gameObject.SetActive (false);
		}
	}

	//　フェード開始
	private IEnumerator StartFead()
	{
		yield return new WaitForSeconds (2);
		sceneCtrl.Fead ();
	}


}
