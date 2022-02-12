using System;
using System.Collections; 
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using UnityEngine.InputSystem; 
using Game; 

public class UIWindowManager : MonoBehaviour
{
	[SerializeField] private Window[] uiWindows;

	private bool hasPressedButton; 
	
	private void Update()
	{
		if(Keyboard.current.escapeKey.isPressed && !hasPressedButton) {
			hasPressedButton = true; 
			PauseMenu();
			if(uiWindows[0].windowObject.activeInHierarchy)
				uiWindows[0].windowObject.SetActive(false);
		}
			
	}
	public void PauseMenu() //Also used as resume button
	{
		Debug.Log("Pause/Unpause");
		
		if(uiWindows[2].windowObject.activeInHierarchy) {
			uiWindows[2].windowObject.SetActive(false);
			Time.timeScale = 1; 
		}
			
		else {
			uiWindows[2].windowObject.SetActive(true);
			Time.timeScale = .001f; 
		}

		StartCoroutine(ButtonDelay()); 
	}

	public void Quit()
	{
		Debug.Log("Quit Button");
		uiWindows[2].windowObject.SetActive(false);
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu");
	}

	private IEnumerator ButtonDelay()
	{
		yield return new WaitForSeconds(.1f);
		hasPressedButton = false; 
	}

	[System.Serializable]
	struct Window
	{
		public string windowName; 
		public GameObject windowObject;
	}
	
}
