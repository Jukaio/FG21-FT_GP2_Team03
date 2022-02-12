using System;
using UnityEngine;
using UnityEditor; 
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class MainMenuManager : MonoBehaviour
{
	[SerializeField] private VoidEvent onMouseOver;
	[SerializeField] private AudioClip buttonSound; 
	
	private AudioSource source; 
	
	
	//Buttons
	public void Play() => SceneManager.LoadScene("Beta");

	public void Options()
	{
		//TODO: add some kind of game options screen
	}
	
	public void Quit()
	{
		Application.Quit();
#if UNITY_EDITOR
		EditorApplication.isPlaying = false; 
#endif
	}

	private void PlayButtonSound()
	{
		source.clip = buttonSound; 
		source.Play();
	}

	private void OnEnable()
	{
		source = GetComponent<AudioSource>(); 
		
		onMouseOver.onEvent += PlayButtonSound; 
	}

	private void OnDisable()
	{
		onMouseOver.onEvent -= PlayButtonSound; 
	}
}
