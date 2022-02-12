using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
	public class IdleState : State
	{
		public ChaseState chaseState;
		public bool canSeeThePlayer = false;
		public PlayerDetection playerDetection;

		public override void EnterCurrentState()
		{
			canSeeThePlayer = false;
		}

		public override void ExitCurrentState()
		{

		}

		public override State RunCurrentState()
		{
			if(!chaseState.IsOnNavMesh) {
				return this;
			}

			if(playerDetection.target != null) 
			{
				canSeeThePlayer = true;
			}

			if(canSeeThePlayer) 
			{
				return chaseState;

			}
			else 
			{
				return this;
			}
		}
	}
}

