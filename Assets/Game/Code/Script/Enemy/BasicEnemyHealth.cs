using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyHealth : MonoBehaviour
{
	[SerializeField] private float knockBackForce = 100; 
	[SerializeField] private float recoveryDelay = .5f; 

	[SerializeField]
	private BucketUI healthBucket;
	[SerializeField]
	private GameObject energyPrefab;
	[SerializeField]
	private OnHitBlinker blinker;
	[SerializeField] private VoidEvent onDeath;
	[SerializeField] private GameObjectEvent onEnemyDeath;
	[SerializeField] private ParticleSystem bloodSplatterVFX;
	private Rigidbody rb;
	private NavMeshAgent agent; 
	private FloatObservable currentHealth;

	void Awake()
    {
		currentHealth = ScriptableObject.CreateInstance<FloatObservable>();
		healthBucket.SetCurrentHealthReference(currentHealth);
		currentHealth.Initialise();
		blinker.CurrentHealth = currentHealth;
		healthBucket.enabled = true;
		blinker.enabled = true;
		currentHealth.Value = healthBucket.Maximum;

    }

	public void TakeDamage(float damage, Transform attacker = null)
	{
		currentHealth.Value -= damage;
		Debug.Log($"currentHealth{currentHealth}");

		bloodSplatterVFX.Play(true);
		/*if(attacker != null)
			GetKnockedBack(attacker);*/

		if(currentHealth.Value <= 0) 
		{
			Die();
		}
	}

	private void GetKnockedBack(Transform attacker)
	{
		//TODO: Add some kind of event that prevents enemy from attacking until recovered
		agent.isStopped = true;
		rb.constraints = RigidbodyConstraints.FreezeRotation; 
		Vector3 knockBackDirection = transform.position - attacker.position;
		rb.velocity += knockBackDirection * knockBackForce;
		StartCoroutine(Recover()); 
	}
	
	private IEnumerator Recover()
	{
		yield return new WaitForSeconds(.1f);
		rb.velocity = Vector3.zero;
		rb.constraints = RigidbodyConstraints.FreezeAll;
		yield return new WaitForSeconds(recoveryDelay);
		agent.isStopped = false; 
	}

	private void Die()
	{
		if(energyPrefab != null) 
		{
			Instantiate(energyPrefab, transform.position, transform.rotation);
		}

		onEnemyDeath.Invoke(gameObject); 
		onDeath?.Invoke();
		Destroy(gameObject);
	}

	private void OnEnable()
	{
		rb = GetComponent<Rigidbody>();
		agent = GetComponent<NavMeshAgent>(); 
	}
}
