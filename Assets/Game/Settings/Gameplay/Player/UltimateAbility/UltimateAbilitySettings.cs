using UnityEngine;

[CreateAssetMenu(fileName = "Ultimate Ability Settings", menuName = "Scriptable Objects/Gameplay/UltimateAbility Settings")]
public class UltimateAbilitySettings : ScriptableObject
{
	[SerializeField] private FloatReference coolDown;

	[Tooltip("Will replace the normal running speed")] [SerializeField]
	private FloatReference buffedSpeed;

	[Tooltip("Will replace normal dash range")] [SerializeField]
	private FloatReference buffedDashLength;

	[Tooltip("Will replace normal damage")] [SerializeField]
	private IntReference buffedDamage;

	[SerializeField] private FloatReference duration;

	[Tooltip("Player recieves this speed instead of normal speed when on dl5")]
	[SerializeField] private FloatReference debuffedSpeed;

	[SerializeField] private FloatReference debuffedDashSpeed; 
	
	[SerializeField] private FloatReference debuffedDashAcceleration; 

	public float CoolDown => coolDown.Value;
	public float BuffedSpeed => buffedSpeed.Value;
	public float BuffedDashLength => buffedDashLength.Value;
	public float Duration => duration.Value;
	public int BuffedDamage => buffedDamage.Value;
	public float DebuffedSpeed => debuffedSpeed.Value;

	public float DebuffedDashSpeed => debuffedSpeed.Value;

	public float DebuffedDashAcceleration => debuffedDashAcceleration.Value;
}
