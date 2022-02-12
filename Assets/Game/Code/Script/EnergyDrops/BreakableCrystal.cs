using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableCrystal : MonoBehaviour
{
	[SerializeField] public VoidEvent crystalBreak;
	
	[SerializeField]
	private GameObject energyPrefab;

	[SerializeField]
	private float durabilty = 2f;


	public void ReduceDruablilty()
	{
		durabilty -= 1f;

		if(durabilty <= 0) 
		{
			Break();
		}
	}

	void Break()
	{
		Instantiate(energyPrefab, transform.position, transform.rotation);

		crystalBreak.Invoke();

		Destroy(gameObject);
	}
}
