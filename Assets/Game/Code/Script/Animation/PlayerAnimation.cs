using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    [SerializeField] private VoidEvent onAttack;
    [SerializeField] private VoidEvent onDash;
    [SerializeField] private VoidEvent onClean;
    [SerializeField] private VoidEvent onPlayerDeath; 
    [SerializeField] private Game.Controller controller;
    
	//Animator
	Animator animator;
    
    void OnEnable()
    {
        onDash.onEvent += DashAnimation;
        onAttack.onEvent += AttackAnimation;
        controller.onHeal += HealAnimation;
        onClean.onEvent += CleanAnimation;
        onPlayerDeath.onEvent += DeathAnimation;
    }
    void OnDisable()
    {
        onDash.onEvent -= DashAnimation;
        onAttack.onEvent -= AttackAnimation;
        controller.onHeal -= HealAnimation;
        onClean.onEvent -= CleanAnimation;
        onPlayerDeath.onEvent -= DeathAnimation;
        animator.SetBool("IsDashing", false);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsHealing", false);
        animator.SetBool("IsCleaning", false);
    }
    
    public void DashAnimation()
    {
        animator.Play("Base Layer.Dash");
        //animator.SetBool("IsDashing", true);
        Debug.Log("dashani");
    }
    public void AttackAnimation()
    {
		animator.Play("Base Layer.Slash1");
        //animator.SetBool("IsAttacking", true);
        Debug.Log("atkani");
    }
    public void HealAnimation(Game.ButtonState state)
    {
        animator.Play("Base Layer.Heal");
        //animator.SetBool("IsHealing", true);
        Debug.Log("healani");
    }
    public void CleanAnimation()
    {
        animator.Play("Base Layer.Clean");
        //animator.SetBool("IsCleaning", true);
        Debug.Log("cleanani");
    }

    public void DeathAnimation()
    {
	    animator.Play("PC_Death");
		//animator.SetBool("IsDead", true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //Finds animator of child object for player
        	animator = gameObject.GetComponentInChildren<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
