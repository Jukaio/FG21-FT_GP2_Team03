using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
	[SerializeField]
    private Animator animator;

	[SerializeField]
	private Game.AttackState attackState;
	[SerializeField]
	private Game.ChaseState chaseState;

    void OnEnable()
    {		
		attackState.TryInitialise();
		chaseState.TryInitialise();
		attackState.onAttack.onEvent += OnAttack;
		chaseState.onMove.onEvent += OnMove;
	}
    void OnDisable()
    {
		attackState.onAttack.onEvent -= OnAttack;
		chaseState.onMove.onEvent -= OnMove;
	}

	public void OnAttack()
	{
		animator.SetTrigger("attack");
	}

	public void OnMove(float speed)
	{
		animator.SetBool("isMoving", speed > 0.0f);
	}
}
