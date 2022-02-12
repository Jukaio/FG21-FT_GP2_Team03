using System;
using UnityEngine;

public class EnemyAudioPlayer : MonoBehaviour
{
	[Header("Events")]
	[SerializeField] private VoidEvent onChaseState; 
	[SerializeField] private VoidEvent onChaseStateExited; 
	[SerializeField] private GameObjectEvent onEnemyDeath; 
	[SerializeField] private VoidEvent onEnemyAttack; 
	
	
	[Header("Components")]
	[SerializeField] private AudioSource sfxSource; 
	[SerializeField] private AudioSource sfxSource2;
	
	private EnemyAudioManager audioManager;

	private bool isMoving; 
	
	private void Update()
	{
		if(isMoving)
			PlayFootStepSfx();
	}

	private void PlayDyingSfx()
	{
		sfxSource.clip = audioManager.audio[1].clip[0]; 
		sfxSource.Play();
		
		//TODO: if time, lower volume over time 
	}

	private void PlayAttackSfx()
	{
		sfxSource.clip = audioManager.audio[0].clip[0];
		sfxSource.Play(); 
	}

	private void PlayFootStepSfx()
	{
		if(sfxSource2.isPlaying)
			return;
		
		sfxSource2.clip = audioManager.audio[2].clip[UnityEngine.Random.Range(0, audioManager.audio[2].clip.Length)];
		sfxSource2.Play();
	}

	private void OnEnemyDeath(GameObject go)
	{
		transform.parent = null; 
		PlayDyingSfx();
		Destroy(gameObject, 4); 
	}
	
	private void OnEnable()
	{
		audioManager = GetComponentInChildren<EnemyAudioManager>();

		//onChaseState.onEvent += () => isMoving = true; 
		//onChaseStateExited.onEvent += () => isMoving = false;
		onEnemyDeath.onEvent += OnEnemyDeath;
		onEnemyAttack.onEvent += PlayAttackSfx; 
	}

	private void OnDisable()
	{
		onEnemyDeath.onEvent -= OnEnemyDeath;
		onEnemyAttack.onEvent -= PlayAttackSfx;
	}
}
