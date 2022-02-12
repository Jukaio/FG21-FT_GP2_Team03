using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freya;

namespace Levels
{
	public class Voxel
	{
		public enum Classification
		{
			Passable,
			Wall,
			Corner,
			Door
		}

		private Classification type;
		private Vector2Int index;
		private Vector3 position;
		private Vector3 normal;

		public Classification Type => type;
		public Vector2Int Index => index;
		public Vector3 Position => position;
		public Vector3 Normal => normal;

		public Voxel(Classification type, Vector2Int index, Vector3 position, Vector3 normal)
		{
			this.type = type;
			this.index = index;
			this.position = position;
			this.normal = normal.normalized;
		}
	}

	//public class WorldSection : MonoBehaviour
	//{
	//	private List<WorldRoom> roomList = new List<WorldRoom>();

	//}

	[RequireComponent(typeof(WorldLayout), typeof(WorldLayoutRoomMerger))]
	public class WorldLayoutWallCalculator : MonoBehaviour
	{
		[SerializeField]
		private Vector2Int wallCount = new Vector2Int(10, 10);
		[SerializeField]
		private bool noWallsBetweenRooms = false;

		public WorldLayout Layout => Merger.Layout;
		public WorldLayoutSettings Settings => Layout.Settings;
		public Vector2Int WallCount => wallCount;
		public bool NoWallsBetweenRooms => noWallsBetweenRooms;
		private WorldLayoutRoomMerger cachedMerger;
		public WorldLayoutRoomMerger Merger
		{
			get
			{
				if(cachedMerger == null) {
					cachedMerger = GetComponent<WorldLayoutRoomMerger>();
				}
				return cachedMerger;
			}
		}

		public bool Generate(out Room[,] layout, out List<Room> rooms)
		{
			if(!Merger.Generate(out layout, out rooms)) {
				return false;
			}

			CalculateWallSize();
			return Calculate(layout, rooms);
		}

		public bool Calculate(Room[,] layout, in List<Room> rooms)
		{
			Vector3 wallSize = Settings.WallThickness.X0Y();

			Filter(layout, rooms, wallSize);

			return true;
		}

		private void Filter(Room[,] layout, List<Room> rooms, Vector3 wallSize)
		{
			var halfSize = wallSize * 0.5f;
			var offset = Settings.RoomSize.X0Y() * 0.5f;

			var counts = new int[]
			{
				wallCount.y,
				wallCount.y,
				wallCount.x,
				wallCount.x
			};

			var expansion = new Vector3[]
			{
				Vector3.Scale(wallSize, Vector3.left),
				Vector3.Scale(wallSize, Vector3.right),
				Vector3.Scale(wallSize, Vector3.forward),
				Vector3.Scale(wallSize, Vector3.back),
			};

			var normals = new Vector3[]
			{
				Vector3.back,
				Vector3.forward,
				Vector3.right,
				Vector3.left,
			};

			foreach(Room room in rooms) {
				foreach(var index in room.Indeces) {
					Voxel[,] wallLayout = new Voxel[wallCount.x, wallCount.y];

					var position = Settings.LayoutIndexToWorld(index);

					var bottomLeft = position - offset + halfSize;
					var topRight = position + offset - halfSize;
					// TODO: Hardcoded - Make it versatile
					var doorPositions = new Vector2Int[]
					{
						Vector2Int.RoundToInt(wallCount / 2),
					};

					var hasDirection = new bool[]{
						false, false, false, false
					};

					var neighbours = new Vector2Int[]
					{
						index + Vector2Int.up,
						index + Vector2Int.down,
						index + Vector2Int.left,
						index + Vector2Int.right,
					};

					var origins = new Vector3[]
					{
						topRight,
						bottomLeft,
						bottomLeft,
						topRight,
					};


					bool SideIsWall(int x, int y)
					{
						return !Settings.InBounds(new Vector2Int(x, y))
							|| layout[x, y] == null
							|| (!NoWallsBetweenRooms
							&& layout[x, y] != room);
					}

					bool IsDoor(int x, int y, Vector2Int neighbourIndex)
					{
						bool isDoor = false;
						foreach(var value in doorPositions) {
							isDoor |= x == value.x || y == value.y;
						}
						return isDoor 
							&& Settings.InBounds(new Vector2Int(neighbourIndex.x, neighbourIndex.y)) 
							&& layout[neighbourIndex.x, neighbourIndex.y] != room
							&& layout[neighbourIndex.x, neighbourIndex.y] != null;
					}

					Vector3 GetIndex(int roomIndex, int wallIndex)
					{
						return origins[roomIndex] + (expansion[roomIndex] * wallIndex);
					}

					List<Vector2Int> potentialCorners = new List<Vector2Int>();
					// TODO: set section index
					for(int i = 0; i < neighbours.Length; i++) {

						var x = neighbours[i].x;
						var y = neighbours[i].y;
						if(SideIsWall(x, y)) {

							for(int j = 1; j < counts[i] - 1; j++) {
								var pos = GetIndex(i, j);
								var wallIndex = PositionToIndex(pos, bottomLeft);
								if(!IsDoor(wallIndex.x, wallIndex.y, new Vector2Int(x, y))) {
									var wall = new Voxel(Voxel.Classification.Wall, wallIndex, pos, normals[i]);
									wallLayout[wallIndex.x, wallIndex.y] = wall;
									room.Add(wall);

								}
								else {
									room.Add(new Voxel(Voxel.Classification.Door, wallIndex, pos, normals[i]));
								}
							}
						}
					}

					//bottom-left
					CalculateCorners(room, wallLayout, bottomLeft, Vector2Int.zero);
					//top-right
					CalculateCorners(room, wallLayout, bottomLeft, wallCount - Vector2Int.one);
					var bottomRightIndex = Vector2Int.zero + new Vector2Int(wallCount.x - 1, 0);
					CalculateCorners(room, wallLayout, bottomLeft, bottomRightIndex);
					var topLeftIndex = Vector2Int.zero + new Vector2Int(0, wallCount.y - 1);
					CalculateCorners(room, wallLayout, bottomLeft, topLeftIndex);
				}
			}

			void CalculateCorners(Room room, Voxel[,] wallLayout, Vector3 bottomLeft, Vector2Int at)
			{
				var neighbours = new Vector2Int[]
				{
					at + Vector2Int.up,
					at + Vector2Int.down,
					at + Vector2Int.left,
					at + Vector2Int.right,
				};
				var has = new List<Vector2Int>();

				static bool InBounds(Voxel[,] wallLayout, Vector2Int neighbor)
				{
					return neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < wallLayout.GetLength(0) && neighbor.y < wallLayout.GetLength(0);
				}

				for(var i = 0; i < neighbours.Length; i++) {
					var neighbor = neighbours[i];
					if(InBounds(wallLayout, neighbor)) {
						if(wallLayout[neighbor.x, neighbor.y] != null) {
							has.Add(neighbor - at);
						}
					}
				}


				if(has.Count == 0) {
					var index = at;
					var position = IndexToPosition(at, bottomLeft);
					var roomIndex = Settings.WorldToLayoutIndex(position);
					var roomCenter = bottomLeft + (Vector3.Scale(wallCount.X0Y(), Settings.WallThickness.X0Y()) * 0.5f);
					roomCenter -= Settings.WallThickness.X0Y() * 0.5f;
					var normal = position - roomCenter;

					if(room.Indeces.Contains(roomIndex)) {
						room.Add(new Voxel(Voxel.Classification.Corner, at, position, normal)); // Make this normal invertible for designer - to flip inner walls and such
					}
				}
				else if(has.Count == 1) {
					var index = has[0] + at;
					var element = wallLayout[index.x, index.y];
					var position = IndexToPosition(at, bottomLeft);
					room.Add(new Voxel(Voxel.Classification.Wall, at, position, element.Normal));
				}
				else if(has.Count == 2) {
					Vector3 normal = Vector3.zero;
					foreach(var element in has) {
						normal += element.X0Y();
					}
					if(normal.magnitude > 1.1f) {
						room.Add(new Voxel(Voxel.Classification.Corner, at, IndexToPosition(at, bottomLeft), normal));
					}
					else {
						room.Add(new Voxel(Voxel.Classification.Wall, at, IndexToPosition(at, bottomLeft), normal));
					}
				}
				else if(has.Count == 3 || has.Count == 4) {
					Debug.Log("Rooms with 3 or 4 neighbours found - not implemented");
				}
			}
		}

		private Vector2Int PositionToIndex(Vector3 position, Vector3 bottomLeft)
		{
			position -= bottomLeft;
			var size = Settings.WallThickness;
			position += Settings.WallThickness.X0Y() * 0.5f;
			var temp = new Vector3(position.x / size.x, 0.0f, position.z / size.y);
			return new Vector2Int((int)temp.x, (int)temp.z);
		}

		private Vector3 IndexToPosition(Vector2Int index, Vector3 bottomLeft)
		{
			var size = Settings.WallThickness;
			var temp = new Vector3(index.x * size.x, 0.0f, index.y * size.y);
			temp += bottomLeft;
			return temp;
		}

		private void CalculateWallSize()
		{
			Settings.WallThickness = Settings.RoomSize / WallCount;
		}
	}
}
