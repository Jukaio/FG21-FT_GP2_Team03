using System; 
using UnityEngine;

public class ActionTimer : MonoBehaviour
{
	private Action timerCallbackFunction;
	public float timer;

	public void SetUpTimer(float time, Action callbackFunction = null)
	{
		this.timer = time;
		
		if(callbackFunction != null)
			this.timerCallbackFunction = callbackFunction; 
	}
	
	private void Update()
	{
		if(timer > 0f) {
			timer -= Time.deltaTime;

			if(Elapsed() && timerCallbackFunction != null)
				timerCallbackFunction(); 
		}
	}

	public bool Elapsed()
	{
		return timer <= 0f; 
	}
}
