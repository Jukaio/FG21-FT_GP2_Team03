using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
#endif

[CreateAssetMenu(fileName = "Exchangable Bucket", menuName = "Scriptable Objects/Gameplay/Bucket/Exchangable Bucket")]

public class ExchangableBucket : ScriptableObject
{
	[SerializeField]
	private Game.Bucket bucket;
	[SerializeField, Tooltip("Over time, gain X")]
	private FloatReference gainRate;
	[SerializeField, Tooltip("Over time, lose X")]
	private FloatReference loseRate;
	[SerializeField]
	private FloatReference timePerRate;

	public void Gain(float multiplier)
	{
		bucket.Current += gainRate.Value * multiplier;
	}
	public void Lose(float multiplier)
	{
		bucket.Current += loseRate.Value * multiplier;
	}

	public float RestToFill => bucket.RestToFill;
	public float Current => bucket.Current;
	public float LoseRate => loseRate.Value;
	public float GainRate => gainRate.Value;
	public float TimePerRate => timePerRate.Value;

	public bool IsEmpty => bucket.IsEmpty;
	public bool IsFull => bucket.IsFull;

	public bool WouldBeTooFull(float value) => bucket.WouldBeTooFull(value);
	public bool WouldBeTooEmpty(float value) => bucket.WouldBeTooEmpty(value);

	public IEnumerator GainingWhile(float multiplier, System.Func<bool> function)
	{
		while(function()) {
			Gain(multiplier);
			yield return new WaitForSeconds(timePerRate.Value);
		}
	}

	public IEnumerator LosingWhile(float multiplier, System.Func<bool> function)
	{
		while(function()) {
			Lose(multiplier);
			yield return new WaitForSeconds(timePerRate.Value);
		}
	}
}

