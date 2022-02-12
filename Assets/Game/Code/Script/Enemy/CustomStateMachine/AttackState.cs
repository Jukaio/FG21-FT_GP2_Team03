using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
	public class AttackState : State
	{
		[SerializeField]
		private PlayerDetection playerDetection;

		[SerializeField]
		private ChaseState chaseState;

		[SerializeField]
		private IdleState idleState;

		[SerializeField]
		private EnemyAttack enemyAttack;

		public VoidEvent onAttack;

		public void TryInitialise()
		{
			if(onAttack == null) {
				onAttack = ScriptableObject.CreateInstance<VoidEvent>();
			}
		}

		private void Awake()
		{
			TryInitialise();
		}

		private void OnEnable()
		{
			
		}

		private void OnDisable()
		{
			
		}

		public override State RunCurrentState()
		{
			if(!chaseState.IsOnNavMesh) {
				enemyAttack.OnAttackEnd();
				return idleState;
			}

			if(enemyAttack.IsAttacking == false) {
				return idleState;
			}
			return this;
			//if(playerDetection.distanceFromTarget > chaseState.stoppingDistance && playerDetection.target != null) 
			//{
				
			//	chaseState.isInAttackRange = false;
			//	return chaseState;
			//}
			//else if(playerDetection.target == null) 
			//{
			//	idleState.canSeeThePlayer = false;
			//	return idleState;
			//}
			//else 
			//{
			//	return this;
			//}
		}

		public override void EnterCurrentState()
		{
			enemyAttack.AttackStart(onAttack.Invoke);
		}

		public override void ExitCurrentState()
		{

		}
	}
}

