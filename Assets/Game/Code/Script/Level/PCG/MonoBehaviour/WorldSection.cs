using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freya;

namespace Levels
{
	public class WorldSection : MonoBehaviour
	{
		[SerializeField]
		private WorldLayoutSettings settings;

		public Vector2Int index;

		private void OnDrawGizmos()
		{
			if(settings == null) {
				return;
			}

			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(Vector3.zero, settings.RoomSize.X0Y() - (settings.WallThickness.X0Y() * 2));
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(Vector3.zero, settings.RoomSize.X0Y());
		}
	}
}
