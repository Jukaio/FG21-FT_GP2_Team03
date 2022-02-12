using System;
using System.Collections; 
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.InputSystem;

namespace Player
{
	public class UltimateAbility : MonoBehaviour
	{
		[Header("UI")] 
		[SerializeField] private Image swordGlowUI;

		[Header("Events")] 
		[SerializeField] private FloatEvent onDl0;
		[SerializeField] private VoidEvent onPlayerBuffed; 
		[SerializeField] private VoidEvent onPlayerBuffEnded;
		[SerializeField] private VoidEvent onPlayerDeath; 
		
		[Header("Settings")] 
		[SerializeField] private UltimateAbilitySettings settings;

		private bool buffAvailable = true;
		private bool isDebuffingPlayer = false; 

		//TODO: Subscribe to event that's invoked somewhere in cleanscript
		private void BuffPlayer(float f)
		{
			if(!buffAvailable || Mathf.RoundToInt(f) > 0)
				return;

			Debug.Log("player buffed");
			
			onPlayerBuffed.Invoke();
			buffAvailable = false;
			swordGlowUI.enabled = true; 
			StartCoroutine(BuffDuration()); 
		}
		private void ResetPlayer()
		{
			onPlayerBuffEnded.Invoke(); 
			swordGlowUI.enabled = false; 
		}

		private IEnumerator BuffDuration()
		{
			yield return new WaitForSeconds(settings.Duration);
			ResetPlayer();
			StartCoroutine(BuffCooldown());
		}

		private IEnumerator BuffCooldown()
		{
			yield return new WaitForSeconds(settings.CoolDown);
			buffAvailable = true; 
		}

		private void Die()
		{
			this.enabled = false; 
		}

		private void OnEnable()
		{
			onDl0.onEvent += BuffPlayer;
			onPlayerDeath.onEvent += Die; 
		}

		private void OnDisable()
		{
			onDl0.onEvent -= BuffPlayer; 
			onPlayerDeath.onEvent -= Die; 
		}
	}

}

