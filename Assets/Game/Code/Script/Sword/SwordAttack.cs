using System;
using UnityEngine;
using UnityEngine.InputSystem; 
using Game;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor; 
using Utility; 
#endif

namespace Player
{

	public class SwordAttack : MonoBehaviour
	{
		[Header("Input")] 
		[SerializeField] private Controller controller;

		[Header("Events")]
		[SerializeField] private SwordEvents events;
		[SerializeField] private VoidEvent onPlayerBuffed; 
		[SerializeField] private VoidEvent onPlayerBuffEnded;
		[SerializeField] private VoidEvent onPlayerDeath;
		[SerializeField] private VoidEvent attackStoppedFailCheck; 
		
		[Header("Settings")]
		[SerializeField] private AttackSettings settings;
		[SerializeField] private UltimateAbilitySettings uASettings; 

		[Header("Stamina Settings")] 
		[SerializeField] private float maxStamina = 100; 
		[Tooltip("How many seconds after the attack before the stamina begins to recharge")]
		[SerializeField] private float staminaRechargeDelay = 1;
		[Tooltip("How many seconds stamina takes to recharge")]
		[SerializeField] private float staminaRechargeTime = 2;

		[Header("Components")] 
		[SerializeField] private LayerMask layerMask;
		[SerializeField] private Vector3 swordHitBox;
		[SerializeField] private LayerMask breakableLayer;
		[HideInInspector] public bool canAttack = true; 
		
		[SerializeField]
		private PlayerLocomotion locomotion;

		private Rigidbody rb; 
		private Camera camera; 
		
		//In case more weapons would be added resulting in need of different attackfunctions
		private delegate void AttackFunction();
		private AttackFunction currentWeaponAttack; 
		
		//Stamina
		private float currentStamina;
		private float staminaDelayTimer = 0;

		//Attack
		private int chainAttack = 1;
		private float chainAttackResetTimer = 0;
		private float attackDelayTimer = 0;
		private int currentDamage; 
		
		//Rotation
		private Vector3 mouseDir;

		private void Start()
		{
			//Value assignment
			//currentWeaponAttack = AttackWithSword; 
			currentStamina = maxStamina;
			currentDamage = settings.Damage; 
		}

		private void Update()
		{
			Timers();
			if(canAttack && locomotion.isAttacking)
				attackStoppedFailCheck.Invoke();
		}

		//TODO: V Find a better way to accomplish this V
		private void Timers()
		{
			//Attack delay
			if(attackDelayTimer > 0)
				attackDelayTimer -= Time.deltaTime; 

			//Chainattack/Combo
			if(chainAttackResetTimer > 0)
				chainAttackResetTimer -= Time.deltaTime;
			else
				chainAttack = 1; //Resets to 1 as damage and lunge force will result in 0 on first hit otherwise
			
			//Stamina recharge
			if(staminaDelayTimer > 0)
				staminaDelayTimer -= Time.deltaTime;
			else if(currentStamina < maxStamina)
				RechargeStamina();
		}

		//Called by OnAttack Event (controller) 
		private void Attack(ButtonState Pressed)
		{
			if(Pressed != ButtonState.Pressed || !canAttack || currentStamina < Mathf.RoundToInt(maxStamina / settings.MaxChainedAttacks))
				return;

			if(attackDelayTimer <= 0) {
				events.onAttack.Invoke();
				//currentWeaponAttack();
			}
		}

		//Called as 'currentWeaponAttack();'
		public void AttackWithSword()
		{
			locomotion.Lunge(settings.AttackLungeForce, chainAttack); //Add a small force in the forward direction of the player on attack

			//Enemy collision detection
			Collider[] enemiesHit = Physics.OverlapBox
				(transform.position + transform.up * 1.5f + transform.forward * settings.AttackRange, swordHitBox, transform.rotation, layerMask);
			Collider[] breakablesHit = Physics.OverlapBox(transform.position + transform.forward * settings.AttackRange, swordHitBox, transform.rotation, breakableLayer);
			//Damage/Blood Application
			foreach(Collider enemy in enemiesHit) {
				//TODO: Apply Damage multiplied with combo
				//TODO: IncreaseBlood
				
				if(enemy.TryGetComponent<BasicEnemyHealth>(out var enemyHealth)) {
					enemyHealth.TakeDamage(currentDamage * chainAttack, transform);
				}
				
				//Debug.Log("Hit " + enemy.name);
			}
			if(enemiesHit.Length > 0) {
				events.onSwordHit.Invoke(enemiesHit.Length);
			}
			else {
				events.onSwordMiss.Invoke();
			}

			foreach(Collider breakble in breakablesHit) 
			{
				if(breakble.TryGetComponent<BreakableCrystal>(out var breakableCrystal)) 
				{
					breakableCrystal.ReduceDruablilty();
				}
			}

			currentStamina -= maxStamina / settings.MaxChainedAttacks;
			staminaDelayTimer = staminaRechargeDelay;
			chainAttackResetTimer = settings.ComboResetTime;
			attackDelayTimer = settings.AttackDelayTime; 
			chainAttack += chainAttack < settings.MaxChainedAttacks ? 1 : 0;
		}

		//Recharges stamina if player hasn't attacked for the full range of staminaRechargeDelay 
		private void RechargeStamina()
		{
			currentStamina = maxStamina;
			//Resetting chainAttack prevents player from getting more than max chainAttacks
			chainAttack = 1;
			chainAttackResetTimer = 0; 
		}

		private void BuffPlayer()
		{
			currentDamage = uASettings.BuffedDamage;
		}

		private void DebuffPlayer()
		{
			currentDamage = settings.Damage; 
		}

		private void Die() => this.enabled = false;

		private void OnEnable()
		{
			controller.Activate();
			controller.onAttack += Attack;

			onPlayerBuffed.onEvent += BuffPlayer;
			onPlayerBuffEnded.onEvent += DebuffPlayer;
			onPlayerDeath.onEvent += Die; 
			
			camera = GameObject.Find("Main Camera").GetComponent<Camera>();
			rb = GetComponent<Rigidbody>(); 
		}

		private void OnDisable()
		{
			controller.onAttack -= Attack;
			onPlayerBuffed.onEvent -= BuffPlayer;
			onPlayerBuffEnded.onEvent -= DebuffPlayer;
			onPlayerDeath.onEvent -= Die; 
		}

#if DEBUG_BUILD
		private void OnDrawGizmos()
		{
			if(settings == null) 
				return;
			
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(transform.position + transform.up * 1.5f + transform.forward * settings.AttackRange, swordHitBox);
		}
		private void OnGUI()
		{
			if(settings == null) {
				return;
			}
			GUILayout.Label($"Current Stamina: {currentStamina}");
			GUILayout.Label($"Stamina Recharge Delay: {staminaDelayTimer}");
			GUILayout.Label($"Chained Attacks: {chainAttack - 1}");
		}
#endif
	}

}
