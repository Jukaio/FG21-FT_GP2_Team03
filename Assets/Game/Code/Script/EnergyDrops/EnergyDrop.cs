using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrop : MonoBehaviour
{
	[SerializeField]
	private Game.Bucket playerEnergy;

	[SerializeField]
	private float energyRestored;

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player") 
		{
			playerEnergy.Current += energyRestored;

			Debug.Log(playerEnergy.Current);

			Destroy(gameObject);
		}
	}
}
