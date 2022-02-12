using UnityEngine;

[CreateAssetMenu(fileName = "Health Settings", menuName = "Scriptable Objects/Gameplay/Health Settings")]
public class HealthSettings : ScriptableObject
{
	[Tooltip("How much damage dl5 causes per time")]
	[SerializeField] private IntReference dirtyDamage;
	[Tooltip("How many seconds between each dl5 hit there is")]
	[SerializeField] private FloatReference dirtyDamageTimer;

	public int DirtyDamage => dirtyDamage.Value;
	public float DirtyDamageTimer => dirtyDamageTimer.Value;
}
