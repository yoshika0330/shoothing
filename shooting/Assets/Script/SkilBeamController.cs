using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkilBeamController : MonoBehaviour {

	private GameObject playerObj;

	public float destroyTime = 3;

	void Start () 
	{
		playerObj = GameObject.Find ("idle_player_basic");
		StartCoroutine (DeleteObj ());
	}

	void Update () 
	{
		transform.position = playerObj.transform.position;
	}

	private IEnumerator DeleteObj()
	{
		yield return new WaitForSeconds (destroyTime);
		Destroy (gameObject);
	}
}
