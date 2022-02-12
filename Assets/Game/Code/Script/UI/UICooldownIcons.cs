using System;
using UnityEngine;
using UnityEngine.UI; 

public class UICooldownIcons : MonoBehaviour
{
	[Header("Events")]
	[SerializeField] private VoidEvent onDash;
	[SerializeField] private VoidEvent onDashEnd;
	[SerializeField] private VoidEvent onAttack; 
	[SerializeField] private VoidEvent onAttackEnd; 
	
	[Header("References")] 
	[SerializeField] private FloatReference dashCooldown; 
	[SerializeField] private FloatReference attackCooldown; 
	
	[Header("Components")]
	[SerializeField] private Image dashIcon; 
	[SerializeField] private Image attackIcon;

	private bool dashReady = true;
	private bool attackReady = true;

	private void Update()
	{
		if(!dashReady)
			FillDashIcon();
		if(!attackReady)
			FillAttackIcon(); 
	}

	private void FillDashIcon()
	{
		dashIcon.fillAmount -= (1 / dashCooldown.Value) * Time.deltaTime;

		if(dashIcon.fillAmount <= 0)
			dashReady = true; 
	}

	private void DepleteDashIcon() => dashIcon.fillAmount = 1; 
	
	private void FillAttackIcon()
	{
		attackIcon.fillAmount -= (1 / attackCooldown.Value) * Time.deltaTime;

		if(attackIcon.fillAmount <= 0)
			attackReady = true;
	}

	private void DepleteAttackIcon() => attackIcon.fillAmount = 1;

	private void OnEnable()
	{
		onDash.onEvent += DepleteDashIcon;
		onDashEnd.onEvent += () => dashReady = false;
		onAttack.onEvent += DepleteAttackIcon;
		onAttackEnd.onEvent += () => attackReady = false; 
	}

	private void OnDisable()
	{
		onDash.onEvent -= DepleteDashIcon;
		onAttack.onEvent -= DepleteAttackIcon;
	}
}
