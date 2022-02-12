using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Game
{
	public class ChaseState : State
	{
		public AttackState attackState;
		public IdleState idleState;
		public bool isInAttackRange;
		public float stoppingDistance = 3f;
		public NavMeshAgent enemy;
		public PlayerDetection playerDetection;

		public FloatEvent onMove;
		[SerializeField] private VoidEvent onChaseState; 
		[SerializeField] private VoidEvent onChaseStateExited; 

		public bool IsOnNavMesh => enemy.isOnNavMesh;

		public void TryInitialise()
		{
			if(onMove == null) {
				onMove = ScriptableObject.CreateInstance<FloatEvent>();
			}
		}

		private void Awake()
		{
			previousPosition = enemy.transform.position;
			TryInitialise();
		}

		private Vector3 previousPosition = Vector3.zero;
		public override State RunCurrentState()
		{
			var target = playerDetection.target;

			onMove?.Invoke(Vector3.Distance(previousPosition, enemy.transform.position));
			previousPosition = transform.position;
			if(!IsOnNavMesh || target == null)
			{
				onMove?.Invoke(0.0f);
				return idleState;
			}

			if(playerDetection.distanceFromTarget > stoppingDistance) 
			{
				enemy.SetDestination(target.transform.position);
			}
			else 
			{
				isInAttackRange = true;
			}



			if(isInAttackRange) 
			{
				enemy.SetDestination(transform.position);
				return attackState;
			}
			else 
			{
				return this;
			}
		}

		public override void EnterCurrentState()
		{
			isInAttackRange = false;
			onChaseState.Invoke(); 
		}

		public override void ExitCurrentState()
		{
			onChaseStateExited.Invoke();
		}
	}
}

