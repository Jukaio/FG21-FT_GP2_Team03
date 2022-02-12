using System;
using Game;
using UnityEngine;

public class Healer : MonoBehaviour
{
	[SerializeField] private Game.Controller controller;
	[SerializeField] private BucketExchangerSettings healthEnergyExchanger;
	[SerializeField] private Bucket health;
	[SerializeField] private HealthSettings healthSettings;
	[SerializeField] private FloatObservable healthObserver; 

	[SerializeField] private FloatEvent onDl5; 
	[SerializeField] private FloatEvent onDl4;
	[SerializeField] private VoidEvent onPlayerDeath;
	[SerializeField] private VoidEvent onHeal; 
	[SerializeField] private VoidEvent onHealEnd; 

	private bool isDraining = false;
	private float drainTimer = 1;

	private void Start()
	{
		drainTimer = healthSettings.DirtyDamageTimer; 
	}
	
	private void OnEnable()
	{
		controller.onHeal += Heal;
		onDl5.onEvent += OnDrainHealth;
		onDl4.onEvent += OnStopDrainHealth;
		onPlayerDeath.onEvent += Die;
		healthObserver.onChange += CheckHealth; 
	}

	private void OnDisable()
	{
		controller.onHeal -= Heal;
		onDl5.onEvent -= OnDrainHealth;
		onDl4.onEvent -= OnStopDrainHealth; 
		onPlayerDeath.onEvent -= Die; 
		healthObserver.onChange -= CheckHealth; 
	}

	private Coroutine cleaningRoutine = null;
	
	public void Heal(Game.ButtonState state)
	{
		if(state == Game.ButtonState.Pressed) {
			if(cleaningRoutine == null) {
				cleaningRoutine = StartCoroutine(healthEnergyExchanger.ExchangingBetaForAlphaRateWhile(() => controller.Heal == Game.ButtonState.Pressed));
				onHeal.Invoke(); 
			}
		}
		else if(state == Game.ButtonState.Released) {
			if(cleaningRoutine != null) {
				StopCoroutine(cleaningRoutine);
				cleaningRoutine = null;
				onHealEnd.Invoke(); 
			}
		}
	}

	private void OnDrainHealth(float f) => isDraining = true;
	
	private void OnStopDrainHealth(float f) => isDraining = false; 

	private void Update()
	{
		DrainTimer(); 
	}

	private void DrainTimer()
	{
		if(!isDraining)
			return; 
			
		if(drainTimer > 0)
			drainTimer -= Time.deltaTime;
		else {
			drainTimer = healthSettings.DirtyDamageTimer; //Reset timer
			DrainHealth(healthSettings.DirtyDamage); //Drain health
		}
	}

	private void DrainHealth(float amount) => 	health.Current -= amount;

	private void CheckHealth(float f)
	{
		if(health.Current <= 0)
			onPlayerDeath.Invoke();
	}

	private void Die()
	{
		this.enabled = false; 
	}
}
