using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	[SerializeField]
	private float attackRange = 1f;

	[SerializeField]
	private Game.Bucket playerHealth;

	[SerializeField]
	private Vector3 attackHitBox;

	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	private float damage;

	[SerializeField]
	private float attackTimer;

	private bool attacking = false;
	public bool IsAttacking => attacking;

	public void AttackStart(System.Action onStart)
	{
		if(!attacking) 
		{
			attacking = true;
			onStart?.Invoke();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position + transform.forward * attackRange, attackHitBox);
	}
	public void OnHit()
	{
		Collider[] playerHit = Physics.OverlapBox(transform.position + transform.forward * attackRange, attackHitBox, transform.rotation, layerMask);

		foreach(Collider player in playerHit) {
			playerHealth.Current -= damage;

			Debug.Log(playerHealth.Current);
		}

	}

	public void OnAttackEnd()
	{
		attacking = false;
	}

	IEnumerator AttackCorutine()
	{
		yield return new WaitForSeconds(attackTimer);

		Collider[] playerHit = Physics.OverlapBox(transform.position + transform.forward * attackRange, attackHitBox, transform.rotation, layerMask);

		foreach(Collider player in playerHit) 
		{
			playerHealth.Current -= damage;
			
			Debug.Log(playerHealth.Current);
		}

		yield return new WaitWhile(() => attacking);
	}

}
