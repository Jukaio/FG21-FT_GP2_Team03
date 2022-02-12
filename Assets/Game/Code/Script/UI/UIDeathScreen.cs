using System;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class UIDeathScreen : MonoBehaviour
{
	[Header("Settings")] 
	[SerializeField] private float fadeDelay; 
	[SerializeField] private float fadeTime; 
	
	[Header("Events")]
	[SerializeField] private VoidEvent onPlayerDeath;
	[SerializeField] private VoidEvent onBossDeath;

	[Header("Components")]
	[SerializeField] private Image deathCover;
	[SerializeField] private Image winCover;

	private bool isDead = false;
	private bool hasWon = false; 
	private float timer; 

	private void Update()
	{
		if(isDead || hasWon) {
			if(fadeDelay > 0)
				fadeDelay -= Time.deltaTime; 
			else if(isDead)
				Cover(deathCover);
			else
				Cover(winCover);
		}
			
	}

	private void Cover(Image cover)
	{
		Color tempColor = cover.color;
		tempColor.a += (1 / fadeTime) * Time.deltaTime;
		cover.color = tempColor;

		if(timer > 0)
			timer -= Time.deltaTime;
		else
			SceneManager.LoadScene("MainMenu");
	}

	private void Die() => isDead = true;
	private void Win() => hasWon = true; 
	
	private void OnEnable()
	{
		timer = fadeTime; 
		onPlayerDeath.onEvent += Die;
		onBossDeath.onEvent += Win; 
	}

	private void OnDisable()
	{
		onPlayerDeath.onEvent -= Die; 
		onBossDeath.onEvent -= Win; 
	}
}
