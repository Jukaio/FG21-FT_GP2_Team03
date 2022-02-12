using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class StateManager : MonoBehaviour
	{
		[SerializeField]
		State currentState;

		void Update()
		{
			RunStateMachine();
		}

		private void RunStateMachine()
		{
			State nextState = currentState?.RunCurrentState();

			if(nextState != currentState && nextState != null) {
				SwitchToNextState(nextState);

			}
		}
		private void SwitchToNextState(State nextState)
		{
			currentState.ExitCurrentState();
			currentState = nextState;
			currentState.EnterCurrentState();
		}
	}
}

