using UnityEngine;
using System.Collections.Generic;

namespace Levels
{
	public class WorldRoom : MonoBehaviour
	{
		private Room room;
		private WorldWalls walls;
		private WorldDoorCollection doors;
		private WorldSectionCollection sections;

		private void Initialise(WorldRoom[,] layout, Room room, WorldBuilderSettings settings, bool isFinalRoom = false)
		{
			this.room = room;
			GameObject sectionsGO = new GameObject("Sections");
			sections = sectionsGO.AddComponent<WorldSectionCollection>();
			sectionsGO.isStatic = true;

			sections.transform.SetParent(transform);
			foreach(var index in room.Indeces) {
				var twoDimArrIndex = settings.LayoutSettings.LayoutIndexToTwoDimensionalArrayIndex(index.x, index.y);
				layout[twoDimArrIndex.x, twoDimArrIndex.y] = this;
				// Create WorldSection
				var section = isFinalRoom ? settings.CreateFinalRoom(index) : settings.CreateSection(index);
				section.transform.SetParent(sections.transform, true);
				sections.Add(section, index);
			}		
			CreateWallsAndCorners(room, settings);
			CreateDoors(room, settings);
		}

		private void CreateDoors(Room room, WorldBuilderSettings settings)
		{
			doors = settings.CreateDoors(room);
			doors?.transform.SetParent(transform, true);
		}

		private void CreateWallsAndCorners(Room room, WorldBuilderSettings settings)
		{
			walls = settings.CreateWallsAndCorners(room);
			walls?.transform.SetParent(transform, true);
		}

		public static WorldRoom CreateFromRoom(WorldRoom[,] layout, Room room, WorldBuilderSettings settings, bool isFinalRoom)
		{
			GameObject child = new GameObject("Room");
			child.isStatic = true;
			var worldRoom = child.AddComponent<WorldRoom>();

			worldRoom.Initialise(layout, room, settings, isFinalRoom);

			return worldRoom;
		}
	}
}


