using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomController : MonoBehaviour {

	//	爆発前のボムを用意
	public GameObject bomObj;

	void Start ()
	{
		StartCoroutine (Exprosion ());
	}
	
	void Update () 
	{
	}

	private IEnumerator Exprosion()
	{
		//	出現して二秒たったら爆発の判定を出す
		yield return new WaitForSeconds (2);
		if (GameController.Instance.gameState ==
			GameController.GameState.Play) 
		{
			bomObj.SetActive (false);
			this.GetComponent<BoxCollider2D> ().enabled = true;
			//　爆発の判定が出てから一秒後に消す
			yield return new WaitForSeconds (1);
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			Destroy(gameObject);
		}
	}

}
