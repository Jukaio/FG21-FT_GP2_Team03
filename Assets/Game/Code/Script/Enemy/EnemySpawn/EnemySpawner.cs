using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Levels
{
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField]
		private GameObject BasicEnemy;

		[SerializeField]
		private int numberOfSpawnsSet;

		[SerializeField]
		private float minNumberOfSpawns;

		[SerializeField]
		private float maxNumberOfSpawns;

		[SerializeField]
		bool spawningIsRandom;

		private NavMeshTriangulation Triangulation;

		[SerializeField]
		private List<Transform> spawnPoints = new List<Transform>();

		[SerializeField]
		private GameObjectEvent onEnemyDeath;

		private bool roomClear = false;
		private int enemyCount = 0; 
		
		void Start()
		{

			onEnemyDeath.onEvent += RemoveDeadEnemyFromList;
			//Triangulation = NavMesh.CalculateTriangulation();
			//spawnPoints.Add(Triangulation.vertices.Length);

			var i = 0;

			if(spawningIsRandom) {
				float amount = Random.Range(minNumberOfSpawns, maxNumberOfSpawns);



				while(i < amount) {
					EnemySpawn();
					i++;
				}
			}
			else {
				while(i < numberOfSpawnsSet) {
					EnemySpawn();
					i++;
				}
			}
			
			if(enemyCount > 0)
				CloseAllDoors();
		}

		private void EnemySpawn()
		{

			var SpawnPointIndex = Random.Range(0, spawnPoints.Count);

			var enemy = Instantiate(BasicEnemy, spawnPoints[SpawnPointIndex].position, transform.rotation);
			enemy.transform.SetParent(transform, true);
		
			enemyCount++; 

			//var VertexIndex = Random.Range(0, Triangulation.vertices.Length);

			////var VertexIndex = spawnPoints.Shuffle;

			//NavMeshHit Hit;

			//if(NavMesh.SamplePosition(Triangulation.vertices[VertexIndex], out Hit, 2f, NavMesh.AllAreas)) 
			//{
			//	Instantiate(BasicEnemy, Hit.position, transform.rotation);

			//	numberOfEnemies.Add(BasicEnemy);
			//}

		}

		void CloseAllDoors()
		{
			
			WorldDoor[] doorList = FindObjectsOfType(typeof(WorldDoor)) as WorldDoor[];

			foreach(var door in doorList) 
			{
				door.CloseDoors();
			}
		}

		void OpenAllDoors()
		{
			Debug.Log("Opening");
			WorldDoor[] doorList = FindObjectsOfType(typeof(WorldDoor)) as WorldDoor[];


			foreach(var door in doorList) 
			{
				door.OpenDoors();
			}

		}

		void RemoveDeadEnemyFromList(GameObject enemy)
		{
			enemyCount--;
			Debug.Log($"Killed enemy: {enemyCount} left.");
			if(enemyCount == 0)
				OpenAllDoors(); 
		}
	}
}

