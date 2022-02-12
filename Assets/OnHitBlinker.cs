using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OnHitBlinker : MonoBehaviour
{
	[SerializeField]
	private FloatObservable currentHealth;
	[SerializeField]
	private float frequency;
	[SerializeField]
	private float duration;
	[SerializeField]
	private Color color;

	public FloatObservable CurrentHealth
	{
		get => currentHealth;
		set => currentHealth = value;
	}

	private float cachedPreviousHealth = 0.0f;
	private List<SkinnedMeshRenderer> filters = new List<SkinnedMeshRenderer>();
	private Coroutine damagedRoutine = null;
	

	private void OnEnable()
	{
		cachedPreviousHealth = currentHealth.Value;
		currentHealth.onChange += OnHealthChange;
		filters = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
	}

	private void OnHealthChange(float value)
	{
		if(cachedPreviousHealth > value) {
			if(damagedRoutine == null) {
				damagedRoutine = StartCoroutine(GettingDamaged());
			}
		}
		cachedPreviousHealth = value;
	}

	private void OnDisable()
	{
		currentHealth.onChange -= OnHealthChange;
		filters.Clear();
	}

	private IEnumerator GettingDamaged()
	{
		float t = 0.0f;
		bool isRed = true;
		List<Color> prevColours = new List<Color>();
		filters.ForEach(x => prevColours.Add(x.material.GetColor("_EmissionColor")));
		while(t < duration) {
			if(isRed) {
				filters.ForEach(filter => filter.material.SetColor("_EmissionColor", color));
			}
			else {
				filters.ForEach(filter => filter.material.SetColor("_EmissionColor", Color.black));
			}
			isRed = !isRed;
			t += frequency;
			yield return new WaitForSeconds(frequency);
		}
		for(int i = 0; i < filters.Count; i++) {
			var filter = filters[i];
			filter.material.SetColor("_EmissionColor", prevColours[i]);
		}
		damagedRoutine = null;
	}
}
