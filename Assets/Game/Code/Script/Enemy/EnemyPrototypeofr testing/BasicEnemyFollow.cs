using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyFollow : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private NavMeshAgent self;
	[SerializeField] private float detectionRange = 10;
	[SerializeField] private LayerMask detectionMask;

	private void Update()
	{
		if(target != null) {
			Debug.Log("Checking distance to target");
			
			float distanceToTarget = Vector3.Distance(transform.position,
				new Vector3(target.position.x, transform.position.y, target.position.z)); 
		
			if(distanceToTarget > self.stoppingDistance)
				MoveToTarget(target.position);
		}
	}

	private IEnumerator CheckForTarget()
	{
		yield return new WaitForSeconds(.5f);
		Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, detectionMask);

		if(hits.Length > 0) {
			target = hits[0].transform; 
			MoveToTarget(hits[0].transform.position);
		}
		
		if(target == null)
			StartCoroutine(CheckForTarget()); 
	}

	private void MoveToTarget(Vector3 targetPosition)
	{
		Vector3 destination = new Vector3(target.position.x, transform.position.y, target.position.z);
		self.SetDestination(destination); 
		
		Debug.Log("Moving to target location");
	}

	private void OnEnable()
	{
		if(target == null)
			StartCoroutine(CheckForTarget());
	}
}
