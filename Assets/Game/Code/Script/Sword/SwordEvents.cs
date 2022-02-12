using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Utility;
#endif
namespace Player
{
	[CreateAssetMenu(fileName = "Sword Events", menuName = "Scriptable Objects/Gameplay/Sword Events")]
	public class SwordEvents : ScriptableObject
	{
		public VoidEvent onAttack;
		public VoidEvent onAttackStopped; 
		public IntEvent onSwordHit;
		public VoidEvent onSwordMiss;
	}

}
