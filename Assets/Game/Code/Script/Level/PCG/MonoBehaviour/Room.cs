using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Levels
{
	// TODO: Class too big :( but eh, never change a running system
	public class Room
	{
		private List<Vector2Int> indeces = new List<Vector2Int>();
		private List<Room> neighbours = new List<Room>();

		private List<Voxel> walls = new List<Voxel>();
		private List<Voxel> corners = new List<Voxel>();
		private List<Voxel> doors = new List<Voxel>();

		private RectInt bounds = new RectInt(0, 0, 0, 0);

		public List<Vector2Int> Indeces => indeces;
		public bool IsEmpty => indeces.Count == 0;
		public bool IsValid => indeces.Count > 0;
		public Vector2Int Origin => indeces[0];
		public List<Room> Neighbours => neighbours;
		public List<Voxel> Doors => doors;
		public List<Voxel> Walls => walls;
		public List<Voxel> Corners => corners;

		// Invariant: indeces count == 0
		public Room(Vector2Int index)
		{
			Add(index);
		}

		// Please only use it with neighbours, for the love of God
		public static Room Merge(Room a, Room b)
		{
			Room that = new Room(a.Origin);
			// Skip 0, already in with origin
			for(var i = 1; i < a.indeces.Count; i++) {
				var index = a.indeces[i];
				that.Add(index);
			}
			// Add all indeces of b
			for(var i = 0; i < b.indeces.Count; i++) {
				var index = b.indeces[i];
				that.Add(index);
			}
			that.neighbours = a.neighbours.Union(b.neighbours).ToList();

			foreach(var neighbour in that.neighbours) {
				neighbour.Remove(a);
				neighbour.Remove(b);
				neighbour.Add(that);
			}
			that.neighbours.Remove(a);
			that.neighbours.Remove(b);

			return that;
		}

		public Room GetRandomNeighbour(System.Func<Room, bool> func)
		{
			var copy = this.neighbours.Where(func).ToList();
			copy.Shuffle();
			return copy.Count > 0 ? copy[0] : null;
		}

		public void Add(Voxel voxel)
		{
			switch(voxel.Type) {
				case Voxel.Classification.Passable:
					break;
				case Voxel.Classification.Wall:
					walls.Add(voxel);
					break;
				case Voxel.Classification.Corner:
					corners.Add(voxel);
					break;
				case Voxel.Classification.Door:
					doors.Add(voxel);
					break;
			}
		}

		public bool TryAdd(Voxel voxel)
		{
			switch(voxel.Type) {
				case Voxel.Classification.Passable:
					return false;
				case Voxel.Classification.Wall:
					if(!walls.Contains(voxel)) {
						walls.Add(voxel);
						return true;
					}
					break;
				case Voxel.Classification.Corner:
					if(!corners.Contains(voxel)) {
						corners.Add(voxel);
						return true;
					}
					break;
				case Voxel.Classification.Door:
					if(!doors.Contains(voxel)) {
						doors.Add(voxel);
						return true;
					}
					break;
			}
			return false;
		}

		public void Remove(Voxel voxel)
		{
			switch(voxel.Type) {
				case Voxel.Classification.Passable:
					break;
				case Voxel.Classification.Wall:
					walls.Remove(voxel);
					break;
				case Voxel.Classification.Corner:
					corners.Remove(voxel);
					break;
				case Voxel.Classification.Door:
					doors.Remove(voxel);
					break;
			}
		}

		public bool TryRemove(Voxel voxel)
		{
			switch(voxel.Type) {
				case Voxel.Classification.Passable:
					return false;
				case Voxel.Classification.Wall:
					if(walls.Contains(voxel)) {
						walls.Remove(voxel);
						return true;
					}
					break;
				case Voxel.Classification.Corner:
					if(corners.Contains(voxel)) {
						corners.Remove(voxel);
						return true;
					}
					break;
				case Voxel.Classification.Door:
					if(doors.Contains(voxel)) {
						doors.Remove(voxel);
						return true;
					}
					break;
			}
			return false;
		}

		public void Add(Vector2Int index)
		{
			bounds.min = Vector2Int.Min(bounds.min, index);
			bounds.max = Vector2Int.Max(bounds.max, index);
			indeces.Add(index);
		}

		public void Remove(Vector2Int index)
		{
			indeces.Remove(index);
		}

		public bool TryAdd(Vector2Int index)
		{
			if(!indeces.Contains(index)) {
				Add(index);
				return true;
			}
			return false;
		}

		public bool TryRemove(Vector2Int index)
		{
			if(indeces.Contains(index)) {
				Remove(index);
				return true;
			}
			return false;
		}

		public void Add(Room neighbour)
		{
			neighbours.Add(neighbour);
		}

		public void Remove(Room neighbour)
		{
			neighbours.Remove(neighbour);
		}

		public bool TryAdd(Room neighbour)
		{
			if(!neighbours.Contains(neighbour)) {
				Add(neighbour);
				return true;
			}
			return false;
		}

		public bool TryRemove(Room neighbour)
		{
			if(neighbours.Contains(neighbour)) {
				Remove(neighbour);
				return true;
			}
			return false;
		}

		public void ForEachIndex(System.Action<Vector2Int> action)
		{
			foreach(var index in indeces) {
				action(index);
			}
		}
	}
}

