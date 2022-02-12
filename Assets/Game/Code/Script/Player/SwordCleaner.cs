using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Utility;
#endif

public class SwordCleaner : MonoBehaviour
{
	[SerializeField] private RectTransformReference rectTransformReference;
	[SerializeField] private Game.Controller controller;
	[SerializeField] private ExchangableBucket dirtBucket;
	[SerializeField] private BucketExchangerSettings dirtEnergyExchanger;

	[SerializeField] private IntEvent onSwordHit;
	[SerializeField] private VoidEvent onClean;
	[SerializeField] private VoidEvent onCleanEnd; 
	[SerializeField] private VoidEvent onPlayerDeath;
	[SerializeField] private IntEvent onDashHit; 

	[SerializeField] private float requiredCleaningAmount = 500.0f;

	[SerializeField]
	private AudioSource voiceLine;
	public AudioSource VoiceLine
	{
		get => voiceLine; 
		set => voiceLine = value;
	}

	[SerializeField]
	private float delayBetweenVoiceLine = 1.0f;
	public float DelayBetweenVoiceLine
	{
		get => delayBetweenVoiceLine;
		set => delayBetweenVoiceLine = value;
	}
	private Coroutine swordTalkingRoutine = null;

	private void OnEnable()
	{
		onSwordHit.onEvent += Dirt;
		onDashHit.onEvent += Dirt; 
		controller.onClean += Clean;
		onPlayerDeath.onEvent += Die; 
	}

	private void OnDisable()
	{
		onSwordHit.onEvent -= Dirt;
		onDashHit.onEvent -= Dirt; 
		controller.onClean -= Clean;
		onPlayerDeath.onEvent -= Die; 
	}

	private Coroutine cleaningRoutine = null;

	public void Clean(Game.ButtonState state)
	{
		if(state == Game.ButtonState.Pressed) {
			if(cleaningRoutine == null) {
				cleaningRoutine = StartCoroutine(Cleaning());
			}
		}
		else if(state == Game.ButtonState.Released) {
			if(cleaningRoutine != null) {
				StopCoroutine(cleaningRoutine);
				cleaningRoutine = null;
				onCleanEnd.Invoke(); 
			}
		}
	}

	public IEnumerator Cleaning()
	{
		// Put animations in here
		while(controller.Clean == Game.ButtonState.Pressed) {
			// ... or here
			var local = rectTransformReference.Value.InverseTransformPoint(controller.Target);
			float distance = 0.0f;
			while(rectTransformReference.Value.rect.Contains(local) && !dirtBucket.IsEmpty) {
				var previous = local;
				local = rectTransformReference.Value.InverseTransformPoint(controller.Target);
				distance += Vector3.Distance(local, previous);
				if(distance > requiredCleaningAmount) {
					distance -= requiredCleaningAmount;
					dirtEnergyExchanger.ExchangeBetaForAlpha();
					if(onClean != null) {
						onClean.Invoke();
					}
				}
				yield return null;
			}
			yield return null;
		}
	}

	public void Dirt(int amount)
	{
		dirtBucket.Lose(amount);
	}

	private void Die()
	{
		this.enabled = false; 
	}
}

