using UnityEngine;

[CreateAssetMenu(fileName = "Dash Settings", menuName = "Scriptable Objects/Gameplay/Dash Settings")]
public class DashSettings : ScriptableObject
{
	[Header("Locomotion")]
	[SerializeField] private FloatReference dashSpeed;
	[Tooltip("Higher dash acceleration will make dashing more snappy")] 
	[SerializeField] private FloatReference dashAcceleration;
	[Tooltip("Longer dashlength will make the dash go for a longer distance")]
	[SerializeField] private FloatReference dashLength;
	[Tooltip("Cooldown in seconds for dashing")]
	[SerializeField] private FloatReference dashCooldown;
	
	[Header("Attack")]
	[SerializeField] private LayerMask hitMask;
	[SerializeField] private Vector3Reference hitBoxSize; 
	[SerializeField] private IntReference damage;

	public float DashSpeed => dashSpeed.Value;
	public float DashAcceleration => dashAcceleration.Value;
	public float DashLength => dashLength.Value;
	public float DashCooldown => dashCooldown.Value;

	public LayerMask HitMask => hitMask;
	public Vector3 HitBoxSize => hitBoxSize.Value;
	public int Damage => damage.Value; 
}
