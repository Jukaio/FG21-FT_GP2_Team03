using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attack Settings", menuName = "Scriptable Objects/Gameplay/Enemy Attack Settings")]
public class EnemyAttackSettings : ScriptableObject
{
	[Header("Attack")]
	[Tooltip("Enemy Damage")]
	[SerializeField]
	private FloatReference damage;
}
