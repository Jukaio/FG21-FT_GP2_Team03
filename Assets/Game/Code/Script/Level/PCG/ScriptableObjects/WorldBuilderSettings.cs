using UnityEngine;
using Freya;

namespace Levels
{

	[CreateAssetMenu(fileName = "WorldBuilderSettings", menuName = "Scriptable Objects/Gameplay/World Builder Settings")]
	public class WorldBuilderSettings : ScriptableObject
	{
		[SerializeField]
		private WorldLayoutSettings settings;

		[SerializeField]
		private Vector3 offset;
		[SerializeField]
		private WorldSection[] roomPrefabs;
		[SerializeField]
		private WorldSection finalRoom;
		[SerializeField]
		private GameObject wallPrefab;
		[SerializeField]
		private GameObject cornerPrefab;
		[SerializeField]
		private GameObject doorPrefab;
		[SerializeField]
		private float teleportOffset = 1.5f;
		
		public Vector3 Offset => offset;
		public WorldSection RoomPrefab => roomPrefabs[Random.Range(0, roomPrefabs.Length)];
		public GameObject WallPrefab => wallPrefab;
		public GameObject DoorPrefab => doorPrefab;
		public GameObject CornerPrefab => cornerPrefab;
		public WorldLayoutSettings LayoutSettings => settings;
		public bool HasFinalRoom => finalRoom != null;

		public WorldSection CreateSection(Vector2Int index)
		{
			var position = settings.LayoutIndexToWorld(index);
			var instance = Instantiate(RoomPrefab, position, Quaternion.identity);
			instance.gameObject.isStatic = true;
			return instance;
		}

		public WorldSection CreateFinalRoom(Vector2Int index)
		{
			var position = settings.LayoutIndexToWorld(index);
			var instance = Instantiate(finalRoom, position, Quaternion.identity);
			instance.gameObject.isStatic = true;
			return instance;
		}

		public WorldWalls CreateWallsAndCorners(Room room)
		{
			if(WallPrefab != null) {
				GameObject wallsGO = new GameObject("Walls and Corners");
				wallsGO.isStatic = true;
				var walls = wallsGO.AddComponent<WorldWalls>();
				//walls.transform.SetParent(child.transform, true); // Do this after return
				foreach(var item in room.Walls) {
					var go = Instantiate(WallPrefab, item.Position + Offset, Quaternion.identity);
					go.transform.forward = item.Normal;
					go.transform.SetParent(walls.transform, true);
					walls.AddWall(go);
				}
				if(CornerPrefab != null) {
					foreach(var item in room.Corners) {
						var go = Instantiate(CornerPrefab, item.Position + Offset, Quaternion.identity);
						go.transform.forward = item.Normal;
						go.transform.SetParent(walls.transform, true);
						walls.AddCorner(go);
					}
				}
				return walls;
			}
			return null;
		}

		public WorldDoorCollection CreateDoors(Room room)
		{
			if(DoorPrefab != null) {
				GameObject doorsGO = new GameObject("Doors");
				var doors = doorsGO.AddComponent<WorldDoorCollection>();
				doorsGO.isStatic = true;
				foreach(var item in room.Doors) {
					var go = Instantiate(DoorPrefab, item.Position + Offset, Quaternion.identity);
					go.transform.forward = item.Normal;
					var tp = (Vector3.Scale(-item.Normal, settings.WallThickness.X0Y() * teleportOffset)) + item.Position;
					go.transform.SetParent(doors.transform, true);
					if(!go.TryGetComponent<WorldDoor>(out var worldDoor)) {
						worldDoor = go.AddComponent<WorldDoor>();
					}
					doors.Add(worldDoor, tp);
				}
				return doors;
			}
			return null;
		}
	}
}

