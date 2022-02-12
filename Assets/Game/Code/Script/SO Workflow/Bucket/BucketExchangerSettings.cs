using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using Utility;

#endif

[CreateAssetMenu(fileName = "Bucket Exchanger Settings", menuName = "Scriptable Objects/Gameplay/Bucket/Bucket Exchanger Settings")]
public class BucketExchangerSettings : ScriptableObject
{
	[SerializeField]
	private ExchangableBucket alpha;
	[SerializeField]
	private ExchangableBucket beta;
	[SerializeField]
	private FloatReference exchangeTimePerRate;

	public float ExchangeTimePerRate => exchangeTimePerRate.Value;

	private static bool CanExchange(ExchangableBucket loser, ExchangableBucket gainer, ref float multiplier)
	{
		System.Func<float, bool> loseChecker = loser.WouldBeTooEmpty;
		System.Func<float, bool> gainChecker = gainer.WouldBeTooEmpty;
		var lossRate = loser.LoseRate;
		var gainRate = gainer.GainRate;
		if(lossRate > 0.0f) {
			loseChecker = loser.WouldBeTooFull;
		}
		if(gainRate > 0.0f) {
			gainChecker = gainer.WouldBeTooFull;
		}

		bool isLoseInvalid = loseChecker(lossRate * multiplier);
		bool isGainInvalid = gainChecker(gainRate * multiplier);
		if(isLoseInvalid || isGainInvalid) {
			// Check if the minimum would go out of bounds
			isLoseInvalid = loseChecker(lossRate);
			isGainInvalid = gainChecker(gainRate);
			if(isLoseInvalid || isGainInvalid) {
				return false;
			}

			// Re-calculate multiplier
			var loserDifference = (lossRate > 0.0f ? loser.RestToFill : loser.Current);
			var gainerDifference = (gainRate > 0.0f ? gainer.RestToFill : gainer.Current);
			multiplier = Mathf.Max(loserDifference / lossRate, gainerDifference / gainRate);
		}
		return true;
	}

	public void ExchangeBetaForAlpha(float multiplier = 1)
	{
		if(CanExchange(beta, alpha, ref multiplier)) {
			beta.Lose(multiplier);
			alpha.Gain(multiplier);
		}
	}

	public void ExchangeAlphaForBeta(float multiplier = 1)
	{
		if(CanExchange(beta, alpha, ref multiplier)) {
			alpha.Lose(multiplier);
			beta.Gain(multiplier);
		}
	}


	public IEnumerator ExchangingBetaForAlphaRateWhile(System.Func<bool> function)
	{
		while(function()) {
			ExchangeBetaForAlpha();
			yield return new WaitForSeconds(exchangeTimePerRate.Value);
		}
	}

	public IEnumerator ExchangingAlphaForBetaRateWhile(System.Func<bool> function)
	{
		while(function()) {
			ExchangeAlphaForBeta();
			yield return new WaitForSeconds(exchangeTimePerRate.Value);
		}
	}
}

