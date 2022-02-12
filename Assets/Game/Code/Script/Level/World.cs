using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Levels
{

	[RequireComponent(typeof(WorldLayoutBuilder))]
	public class World : MonoBehaviour
	{
		[SerializeField]
		private Game.Controller controller;

		[SerializeField]
		private RuntimeWorldReference worldReference;
		[SerializeField]
		private Transform player;
		[SerializeField]
		private FloatEvent onFade;
		[SerializeField]
		private Vector3Event onPlayerTeleport;
		[SerializeField]
		private float fadeDuration = 2.0f;
		private WorldRoom[,] WorldRoomLayout => WorldBuilder.WorldRoomLayout;
		private WorldRoom activeRoom = null;
		private WorldLayoutBuilder cachedWorldBuilder;
		public WorldLayoutBuilder WorldBuilder
		{
			get
			{
				if(cachedWorldBuilder == null) {
					cachedWorldBuilder = GetComponent<WorldLayoutBuilder>();
				}
				return cachedWorldBuilder;
			}
		}
		private NavMeshSurface navMeshSurface;
		public NavMeshSurface NavigationSurface
		{
			get
			{
				if(navMeshSurface == null) {
					navMeshSurface = GetComponent<NavMeshSurface>();
				}
				return navMeshSurface;
			}
		}
		public WorldLayoutSettings settings => WorldBuilder.LayoutSettings;
		public WorldRoom ActiveRoom => activeRoom;

		void Awake()
		{
			worldReference.Value = this;
			WorldBuilder.Build();
		}

		private void OnEnable()
		{
			onPlayerTeleport.onEvent += OnTeleport;
			onPlayerTeleport?.Invoke(player.position);
		}

		private void OnDisable()
		{
			onPlayerTeleport.onEvent -= OnTeleport;
		}

		private void OnTeleport(Vector3 tpPoint)
		{
			var index = settings.WorldToLayoutIndex(tpPoint);
			var i = settings.LayoutIndexToTwoDimensionalArrayIndex(index.x, index.y);
			var previous = activeRoom;
			activeRoom = WorldRoomLayout[i.x, i.y];
			if(activeRoom != previous) {
				StartCoroutine(Fading(activeRoom, previous, tpPoint));
			}
		}

		private IEnumerator Fading(WorldRoom current, WorldRoom previous, Vector3 teleportPosition)
		{
			NavigationSurface.enabled = false;

			float fraction = 1 / fadeDuration;

			float t = 0.0f;
			if(previous != null) {
				while(t < 1.0f) {
					onFade?.Invoke(t);
					t += Time.deltaTime * fraction;
					yield return null;
				}
			}
			t = 1.0f;
			onFade?.Invoke(t);

			previous?.gameObject.SetActive(false);
			current?.gameObject.SetActive(true);
			player.position = teleportPosition;
			NavigationSurface.BuildNavMesh();

			while(t > 0.0f) {
				onFade?.Invoke(t);
				t -= Time.deltaTime * fraction;
				yield return null;
			}
			t = 0.0f;
			onFade?.Invoke(t);

			NavigationSurface.enabled = true;
		}
	}
}
