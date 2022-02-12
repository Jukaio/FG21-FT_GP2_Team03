using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Game;

#if UNITY_EDITOR
using UnityEditor; 
#endif
using Utility; 

namespace Player
{
	[RequireComponent(typeof(PlayerLocomotion))]
	public class PlayerDash : MonoBehaviour
	{
		[Header("Input")]
		[SerializeField] private Controller controller;

		[Header("Events")] 
		[SerializeField] private VoidEvent onPlayerBuffed; 
		[SerializeField] private VoidEvent onPlayerBuffEnded;
		[SerializeField] private VoidEvent onDashEnd; 

		[Header("Settings")] 
		[SerializeField] private DashSettings dashSettings;
		[SerializeField] private UltimateAbilitySettings uASettings; 
		[SerializeField] private VoidEvent onDash;
		[SerializeField] private SwordAttack swordAttack;
		[SerializeField] private VoidEvent onPlayerDeath;
		[SerializeField] private IntEvent onDashHit; 

		private PlayerLocomotion locomotion;

		private bool hasDashed = false;
		private bool isDashing = false;
		private bool dashHit = false; 
		private float calculatedDashTime;
		private float dashTimer;

		private float currentDashLength; 

		private void Start()
		{
			dashTimer = dashSettings.DashCooldown;
			currentDashLength = dashSettings.DashLength; 
		}

		private void Update()
		{
			if(isDashing) {
				DashCooldown();
				if(!dashHit)
					CheckDashHitBox();				
			}
		}

		private void Dash(ButtonState button)
		{
			//Check that the button isn't held which would allow for infinite dashing
			if(isDashing || button != ButtonState.Pressed || locomotion.isAttacking)
				return;

			//playerCollider.enabled = false; 
			//7 = Player 
			Physics.IgnoreLayerCollision(7, 6, true);
			
			locomotion.AssignDashValues(dashSettings.DashSpeed, dashSettings.DashAcceleration); 

			//Calculates approximate dashtime by dashlength and speed
			calculatedDashTime = currentDashLength / locomotion.currentSpeed; //speed (u/s) / units
			
			isDashing = true;
			swordAttack.canAttack = false; //Makes sure player can't attack during dash
			onDash.Invoke();
		}

		private void DashCooldown()
		{
			calculatedDashTime -= Time.deltaTime;
			hasDashed = calculatedDashTime <= 0;

			//Dash cooldown starts counting down to 0, when hits 0 everything is reset. 
			if(hasDashed) {
				locomotion.ResetValues();
				swordAttack.canAttack = true; //Makes sure player can attack after dash
				dashTimer -= Time.deltaTime;
				onDashEnd.Invoke();
				if(dashTimer <= 0) {
					ResetValues(); 
				}
			}
		}

		private void CheckDashHitBox()
		{
			Collider[] hits;
			hits = Physics.OverlapBox(transform.position, dashSettings.HitBoxSize, transform.rotation, dashSettings.HitMask);

			for(int i = 0; i < hits.Length; i++) 
				hits[i].GetComponent<BasicEnemyHealth>().TakeDamage(dashSettings.Damage, transform);

			if(hits.Length > 0) 
				dashHit = true;
			
			onDashHit.Invoke(hits.Length);
		}

		private void ResetValues()
		{
			isDashing = false;
			hasDashed = false;
			dashHit = false; 
			dashTimer = dashSettings.DashCooldown;
			//7 = Player 
			Physics.IgnoreLayerCollision(7, 6, false);
			//playerCollider.enabled = true; 
		}

		public void BuffPlayer()
		{
			currentDashLength = uASettings.BuffedDashLength;
		}

		public void DebuffPlayer()
		{
			currentDashLength = dashSettings.DashLength; 
		}

		private void Die()
		{
			this.enabled = false; 
		}

		private void OnEnable()
		{
			controller.Activate();
			controller.onDodge += Dash;

			locomotion = GetComponent<PlayerLocomotion>();

			onPlayerBuffed.onEvent += BuffPlayer;
			onPlayerBuffEnded.onEvent += DebuffPlayer;
			onPlayerDeath.onEvent += Die; 
		}

		private void OnDisable()
		{
			controller.onDodge -= Dash;
			onPlayerBuffed.onEvent -= BuffPlayer;
			onPlayerBuffEnded.onEvent -= DebuffPlayer;
			onPlayerDeath.onEvent -= Die; 
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(transform.position, dashSettings.HitBoxSize); 
		}
	}
}
