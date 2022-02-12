using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
	public class WorldWalls : MonoBehaviour
	{
		private List<GameObject> walls = new List<GameObject>();
		private List<GameObject> corners = new List<GameObject>();	

		public void AddWall(GameObject wall)
		{
			walls.Add(wall);
		}

		public void AddCorner(GameObject corner)
		{
			corners.Add(corner);
		}
	}
}


