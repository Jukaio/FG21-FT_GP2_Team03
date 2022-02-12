using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Utility;
#endif
namespace Player
{
	[CreateAssetMenu(fileName = "Attack Settings", menuName = "Scriptable Objects/Gameplay/Attack Settings")]
	public class AttackSettings : ScriptableObject
	{
		[Header("General Settings")]
		[SerializeField] private IntReference damage;
		[Tooltip("On attack, player lunges forward with the force of attackLungeForce * chainedAttacks")]
		[SerializeField] private FloatReference attackLungeForce;
		[Tooltip("How many seconds between each attack there is (can be set to 0)")]
		[SerializeField] private FloatReference attackDelayTime;
		[Tooltip("How far forward the player is able to reach enemies")]
		[SerializeField] private FloatReference attackRange;

		[Header("Chainattack Settings")]
		[Tooltip("How many attacks can player do at a time before having to recharge stamina")]
		[SerializeField] private IntReference maxChainedAttacks;
		[SerializeField] private FloatReference comboResetTime;
		[SerializeField] private LayerMask layerMask;

		public int Damage => damage.Value;
		public float AttackLungeForce => attackLungeForce.Value;
		public float AttackDelayTime => attackDelayTime.Value;
		public float AttackRange => attackRange.Value;
		public int MaxChainedAttacks => maxChainedAttacks.Value;
		public float ComboResetTime => comboResetTime.Value;
		public LayerMask LayerMask => layerMask;
	}

}
