using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif
using System.Linq;

namespace Levels
{
	[RequireComponent(typeof(WorldLayout))]
	public class WorldLayoutRoomMerger : MonoBehaviour
	{
		[SerializeField, Range(0.0f, 1.0f)]
		private float mergeChance = 0.5f;
		[SerializeField, Range(1, 10)]
		private int maximumRoomsToMerge = 3;
		[SerializeField]
		private bool carveInWall = false;


		public float MergeChance => mergeChance;
		public int MaximumRoomsToMerge => maximumRoomsToMerge;
		public bool CarveInWall => carveInWall;
		public WorldLayoutSettings Settings => Layout.Settings;
		private WorldLayout cachedLayout;
		
		public WorldLayout Layout
		{
			get
			{
				if(cachedLayout == null) {
					cachedLayout = GetComponent<WorldLayout>();
				}
				return cachedLayout;
			}
		}

		public bool Generate(out Room[,] layout, out List<Room> rooms)
		{
			rooms = null;
			if(Layout.Generate(Layout.Center, out layout, out var originalRooms)) {
				rooms = new List<Room>();
				HashSet<Room> closed = new HashSet<Room>();

				for(var i = 0; i < originalRooms.Count; i++) {
					var room = originalRooms[i];
					if(!closed.Contains(room)) {
						if(Random.Range(0.0f, 1.0f) < MergeChance) {
							if(TryMerge(room, out var merged, closed, MergeChance, MaximumRoomsToMerge - 1)) {
								rooms.Add(merged);
								foreach(var index in merged.Indeces) {
									layout[index.x, index.y] = merged;
								}
							}
						}
					}
				}
				var unmergedRooms = originalRooms.Where((that) => !closed.Contains(that)).ToList();
				rooms.AddRange(unmergedRooms);
				return true;
			}
			return false;
		}

		private static bool TryMerge(in Room room, out Room result, HashSet<Room> closed, float mergeChance, int counter)
		{
			if(counter == 0) {
				result = room;
				return false;
			}

			if(room.IsValid) {
				var neighbour = room.GetRandomNeighbour(that => !closed.Contains(that));
				if(neighbour != null) {
					result = Room.Merge(room, neighbour);
					closed.Add(neighbour);
					closed.Add(room);
					closed.Add(result);

					if(Random.Range(0.0f, 1.0f) < mergeChance) {
						_ = TryMerge(result, out result, closed, mergeChance, counter - 1);		
					}
					return true;
				}
			}
			result = room;
			return false;
		}
	}
}

