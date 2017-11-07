using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

	public GameObject fadePanel;

	public string[] sceneName;
	[HideInInspector]
	public float alfa = 0;
	public float feadSpeed = 0.02f;
	public int sceneNo;

	private bool playStart = false;

	void Update () 
	{
		// タイトル画面の時
		if (SceneManager.GetActiveScene().name == "TitleScene") 
		{
			// エンターキーが押されたらゲームスタート
			if (Input.GetKeyDown (KeyCode.Return)) {
				playStart = true;
			}
			// "R"キーが押されたらスコアをリセット
			else if (Input.GetKeyDown (KeyCode.R)) 
			{
				PlayerPrefs.DeleteAll ();
			}
		}
		if (playStart) 
		{
			Fead ();
		}
	}

	public void Fead()
	{
		if(alfa <= 1)
		{
			fadePanel.GetComponent<Image> ().color = new Color (0, 0, 0, alfa);
			alfa += feadSpeed;
		}
		else if(alfa >= 1)
		{
			SceneManager.LoadScene (sceneName [sceneNo]);
		}
	}
}
