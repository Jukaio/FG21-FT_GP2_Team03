using UnityEngine;

[CreateAssetMenu(fileName = "Locomotion Settings", menuName = "Scriptable Objects/Gameplay/Locomotion Settings")]
public class LocomotionSettings : ScriptableObject
{
	[Header("Movement")] 
	[Tooltip("Character max speed")]
	[SerializeField] private FloatReference speed;
	[Tooltip("Movement speed when attacking")]
	[SerializeField] private FloatReference attackingSpeed; 
	[Tooltip("Character will reach max speed quicker if acceleration is higher")]
	[SerializeField] private FloatReference acceleration;
	[Tooltip("lower decceleration will make the character stop quicker")]
	[SerializeField] [Range(.99f, .01f)] private FloatReference decceleration;
	[Tooltip("Lower 'rapid decceleration' will make 180 degree turns more 'snappy'")]
	[SerializeField] [Range(.99f, .01f)] private FloatReference rapidDecceleration;
	[Tooltip("Player movement speed while cleaning")]
	[SerializeField] private FloatReference cleaningSpeed; 
	[Tooltip("Player movement speed while healing")]
	[SerializeField] private FloatReference healingSpeed; 
	
	[Header("Rotation")] 
	[Tooltip("If true, player rotates automatically to the direction the player is walking in.")]
	[SerializeField] private BoolReference byKeyboard;

	public float Speed => speed.Value;
	public float AttackingSpeed => attackingSpeed.Value; 
	public float Acceleration => acceleration.Value;
	public float Decceleration => decceleration.Value;
	public float RapidDecceleration => rapidDecceleration.Value;
	public float CleaningSpeed => cleaningSpeed.Value;
	public float HealingSpeed => healingSpeed.Value; 
	
	public bool ByKeyboard => byKeyboard.Value; 

}
