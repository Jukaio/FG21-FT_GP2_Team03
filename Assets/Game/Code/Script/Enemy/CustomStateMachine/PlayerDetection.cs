using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
	
	public Transform target;
	public float distanceFromTarget;
	public float forwardOffset = 4.5f;

	private void Update()
	{
		if(target == null) 
		{
			return;
		}

		distanceFromTarget = Vector3.Distance(target.transform.position, transform.position + (transform.forward * forwardOffset));

	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(transform.position + (transform.forward * forwardOffset), 1.0f);
		Gizmos.DrawWireSphere(transform.position + (transform.forward * forwardOffset), distanceFromTarget);
	}

	private void OnTriggerEnter(Collider other)
	{
		
		if(other.gameObject.CompareTag("Player"))
		{
			
			target = other.gameObject.transform;
		}
	}

	
}
