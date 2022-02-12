using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BloodFloatSO : ScriptableObject
{
	public float value;

	public float Value
	{
		get => value;
	}


	public void ChangeAmoutBy(float changeBy)
	{
		value += changeBy;
		value = Mathf.Clamp(value, 0f, value);
	}

	public void SetNewAmout(float newAmount)
	{
		value = newAmount;
		value = Mathf.Clamp(value, 0f, value);
	}
}
