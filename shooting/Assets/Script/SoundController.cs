using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	public AudioClip textSe;
	public AudioClip countSe;
	public AudioClip countGoSe;
	public AudioClip skillChargeSe;
	public AudioClip shotSe;
	public AudioClip skillSe;
	public AudioClip enemyHitSe;
	public AudioClip playerHitSe;
	public AudioClip mainBgm;
	public AudioClip gameoverBgm;
	public AudioClip getItemSe;

	private AudioSource sound01; // システム音
	private AudioSource sound02; // Playerの攻撃音
	private AudioSource sound03; // Playerのヒット音
	private AudioSource sound04; // enemyのヒット音
	private AudioSource sound05; // BGM類

	public static SoundController Instance;
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
	void Start () 
	{
		AudioSource[] audioSource = GetComponents<AudioSource>();
		sound01 = audioSource [0];
		sound02 = audioSource [1];
		sound03 = audioSource [2];
		sound04 = audioSource [3];
		sound05 = audioSource [4];
	}

	public void PlayTextSE()
	{
		sound01.clip = textSe;
		sound01.Play ();
	}

	public void PlayCountSE()
	{
		sound01.clip = countSe;
		sound01.Play ();
	}

	public void PlayCountGoSE()
	{
		sound01.clip = countGoSe;
		sound01.Play ();
	}

	public void PlaySkillChargeSE()
	{
		sound01.clip = skillChargeSe;
		sound01.Play ();
	}

	public void PlayShotSE()
	{
		sound02.clip = shotSe;
		sound02.Play ();
	}

	public void PlaySkillSE()
	{
		sound02.clip = skillSe;
		sound02.Play ();
	}
		
	public void PlayPlayerHitSE()
	{
		sound03.clip = playerHitSe;
		sound03.Play ();
	}

	public void PlayGetItemSE()
	{
		sound03.clip = getItemSe;
		sound03.Play ();
	}

	public void PlayEnemyHitSE()
	{
		sound04.clip = enemyHitSe;
		sound04.Play ();
	}
		
	public void PlayMainBGM()
	{
		sound05.clip = mainBgm;
		sound05.Play ();
	}

	public void PlayGameOverBGM()
	{
		sound05.clip = gameoverBgm;
		sound05.Play ();
	}
}
