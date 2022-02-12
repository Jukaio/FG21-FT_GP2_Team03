using UnityEngine;
using System.Collections.Generic;

namespace Levels
{
	public class WorldDoorCollection : MonoBehaviour
	{
		List<WorldDoor> doors = new List<WorldDoor>();

		public void Add(WorldDoor door, Vector3 tp)
		{
			door.teleportPoint = tp;
			doors.Add(door);
		}
	}
}


