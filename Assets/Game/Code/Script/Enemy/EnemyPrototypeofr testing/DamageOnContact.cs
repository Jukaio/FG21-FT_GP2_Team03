using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
	public class DamageOnContact : MonoBehaviour
	{

		//Dont know how to reference health.
		[SerializeField]
		private Game.Bucket playerHealth;

		[SerializeField]
		int damage = 5;
		private void OnCollisionEnter(Collision other)
		{
			if(other.gameObject.tag == "Player") 
			{
				playerHealth.Current -= damage;
				Debug.Log(playerHealth.Current);
			}
		}
	}
}


