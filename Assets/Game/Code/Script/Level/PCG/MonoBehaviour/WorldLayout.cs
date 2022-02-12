using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Freya;

namespace Levels
{


	public class WorldLayout : MonoBehaviour
	{
		[SerializeField, Range(1, 20)] 
		private int iterations = 2;
		[SerializeField, Range(1, 20)]
		private int roomCountPerIteration = 8;
		[SerializeField]
		private bool ignoreDoubleSteps = true;
		[SerializeField]
		private WorldLayoutSettings worldLayoutSettings;


		public WorldLayoutSettings Settings => worldLayoutSettings;
		public RectInt Rect => worldLayoutSettings.Rect;
		public int Iterations => iterations;
		public int RoomCountPerIteration => roomCountPerIteration;
		public Vector2 RoomSize => worldLayoutSettings.RoomSize;
		public Vector2Int Size => worldLayoutSettings.Size;
		public Vector2Int Center => worldLayoutSettings.Center;
		public Vector2 WallThickness => worldLayoutSettings.WallThickness;

		public bool Generate(Vector2Int start, out Room[,] layout, out List<Room> rooms)
		{
			List<bool[,]> iterations = new List<bool[,]>();
			for(int i = 0; i < Iterations; i++) {
				iterations.Add(RecursiveBacktracking.BuildPath(start, Size, RoomCountPerIteration, out _, ignoreDoubleSteps));
			}

			if(!(iterations.Count > 0)) {
				layout = null;
				rooms = null;
				return false;
			}

			var width = Size.x;
			var height = Size.y;

			for(int index = 1; index < iterations.Count; index++) {
				for(int x = 0; x < width; x++) {
					for(int z = 0; z < height; z++) {
						iterations[0][x, z] |= iterations[index][x, z];
					}
				}
			}


			// Build Rooms
			Room[,] lookUp = new Room[width, height];
			rooms = new List<Room>();

			for(int x = 0; x < width; x++) {
				for(int z = 0; z < height; z++) {
					if(iterations[0][x, z]) {
						var that = new Room(new Vector2Int(x, z));
						rooms.Add(that);
						lookUp[x, z] = that;
					}
				}
			}

			// Set Neighbours
			foreach(var room in rooms) {
				var index = room.Origin;

				bool InBounds(Vector2Int that)
				{
					return that.x >= 0
						&& that.y >= 0
						&& that.x < width
						&& that.y < height;
				}

				bool IsValid(Vector2Int other)
				{
					return InBounds(other)
						&& (lookUp[other.x, other.y] != null);
				}

				if(IsValid(index + Vector2Int.left)) {
					room.Add(lookUp[index.x - 1, index.y]);
				}
				if(IsValid(index + Vector2Int.down)) {
					room.Add(lookUp[index.x, index.y - 1]);
				}
				if(IsValid(index + Vector2Int.right)) {
					room.Add(lookUp[index.x + 1, index.y]);
				}
				if(IsValid(index + Vector2Int.up)) {
					room.Add(lookUp[index.x, index.y + 1]);
				}
			}
			layout = lookUp;
			return true;
		}
	}
}

