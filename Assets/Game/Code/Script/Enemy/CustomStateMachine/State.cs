using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public abstract class State : MonoBehaviour
	{
		public abstract void EnterCurrentState();
		public abstract State RunCurrentState();
		public abstract void ExitCurrentState();

	}
}

