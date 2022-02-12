using UnityEngine;
using Freya;

namespace Levels
{

	[CreateAssetMenu(fileName = "WorldLayoutSettings", menuName = "Scriptable Objects/Gameplay/World Layout Settings")]
	public class WorldLayoutSettings : ScriptableObject
	{
		[SerializeField]
		private RectInt rect = new RectInt(0, 0, 10, 10);
		[SerializeField]
		private Vector2Observable roomSize;
		[SerializeField]
		private Vector2Observable wallThickness;
		[SerializeField]
		private BoolObservable showDebugIndeces;

		public RectInt Rect => rect;
		public Vector2 RoomSize => roomSize.Value;
		public Vector2Int Size => Rect.max - Rect.min;
		public Vector2Int Center => Size / 2;
		public Vector2 WallThickness
		{
			get => wallThickness.Value; 
			set => wallThickness.Value = value;
		}
		public bool ShowDebugIndeces => showDebugIndeces.Value;

		public Vector2Int WorldToLayoutIndex(Vector3 position)
		{
			position -= RoomSize.X0Y() * 0.5f;
			position = new Vector3(position.x / RoomSize.x, position.y, position.z / RoomSize.y);
			return Vector2Int.RoundToInt(new Vector2(position.x, position.z));
		}

		public Vector3 LayoutIndexToWorld(Vector2Int index)
		{
			var position = new Vector3(index.x, 0, index.y);
			position = Vector3.Scale(position, new Vector3(RoomSize.x, 1.0f, RoomSize.y));
			position += RoomSize.X0Y() * 0.5f;
			return position;
		}

		public bool InBounds(Vector2Int index)
		{
			return Rect.Contains(index);
		}

		public Vector2Int TwoDimensionalArrayIndexToLayoutIndex(int x, int y)
		{
			Vector2Int index = new Vector2Int(x, y);
			return index + Rect.min;
		}

		public Vector2Int LayoutIndexToTwoDimensionalArrayIndex(int x, int y)
		{
			Vector2Int index = new Vector2Int(x, y);
			return index - Rect.min;
		}

	}
}

