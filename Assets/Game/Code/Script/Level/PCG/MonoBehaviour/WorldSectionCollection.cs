using UnityEngine;
using System.Collections.Generic;

namespace Levels
{
	public class WorldSectionCollection : MonoBehaviour
	{
		private List<WorldSection> sections = new List<WorldSection>();

		public void Add(WorldSection section, Vector2Int index)
		{
			section.index = index;
			sections.Add(section);
		}
	}
}


