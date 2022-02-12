using UnityEngine;
using UnityEngine.InputSystem;
using Freya;
using Game;
using Plane = UnityEngine.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using System.Collections; 

namespace Player
{
	public class PlayerLocomotion : MonoBehaviour
	{
		[Header("Input")] [SerializeField] private Controller controller;

		[Header("Settings")] 
		[SerializeField] private LocomotionSettings locomotionSettings;
		[SerializeField] private UltimateAbilitySettings uASettings; 
		[SerializeField] private LayerMask projectionMask;

		[Header("Events")] 
		[SerializeField] private SwordEvents events;
		[SerializeField] private VoidEvent onPlayerBuffed; 
		[SerializeField] private VoidEvent onPlayerBuffEnded;
		[SerializeField] private VoidEvent onClean; 
		[SerializeField] private VoidEvent onCleanEnd;
		[SerializeField] private VoidEvent onHeal; 
		[SerializeField] private VoidEvent onHealEnd; 
		[SerializeField] private FloatEvent onDl5; 
		[SerializeField] private FloatEvent onDl4;
		[SerializeField] private VoidEvent onPlayerDeath;
		[SerializeField] private Vector3Event onMove;
		[SerializeField] private VoidEvent attackStoppedFailCheck; 

		//Components
		private Rigidbody rb;
		private Transform cameraTransform;
		private Camera camera;

		//Movement
		private Vector3 direction;
		private Vector3 dashDirection;
		[HideInInspector] public float currentSpeed;
		public bool isAttacking;

		private Ray directionRay;
		private bool isGrounded = false;

		//Orientiation
		private Vector3 mouseDir;
		private Vector3 dashMouseDir;

		private Vector3 lookDir = Vector3.forward;

		//Dashing
		private bool isDashing = false;
		private bool dashingByKeyboard = false; 
		[HideInInspector] public float dashAcceleration;
		
		private Animator animator;

		private bool isBuffed = false;
		private bool isDebuffed = false;
		private bool hasResetValues = true;
		private bool hasDashed = false; 

		private void Awake()
		{
			currentSpeed = locomotionSettings.Speed;
		}

		private void Update()
		{
			CheckIfGrounded();
			HandleDrag();
		}
		
		private void CheckIfGrounded()
		{
			isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.7f);
		}

		private void HandleDrag()
		{
			if(!isGrounded) {
				rb.drag = .01f;
				//rb.velocity = new Vector3(rb.velocity.x, -9.81f, rb.velocity.z);
			}
			else {
				//rb.velocity = new Vector3(rb.velocity.x, -9.81f, rb.velocity.z); 
				rb.drag = 1;
			}
		}

		private void FixedUpdate()
		{
			Move();
			Rotate();
		}

		private void Move()
		{
			//Camera axles
			Vector3 cameraForward = cameraTransform.forward.FlattenY();
			Vector3 cameraRight = cameraTransform.right.FlattenY();
			cameraForward.Normalize();
			cameraRight.Normalize();

			//Direction
			Vector3 oldDirection = rb.velocity.normalized;
			Vector2 inputDirection = controller.Move;
			Vector3 newDirection =
				!isDashing ? cameraForward * inputDirection.y + cameraRight * inputDirection.x : oldDirection;

			direction = inputDirection != Vector2.zero ? Vector3.Lerp(oldDirection, newDirection, .85f) : Vector3.zero;
			Vector3 normalizedCurrentDirection = direction.normalized;
			float directionDelta = Vector3.Dot(oldDirection, normalizedCurrentDirection);

			//Wall detection
			RaycastHit dirHit;
			directionRay.origin = transform.position;
			directionRay.direction = direction;

			//Prevents player from getting stuck on walls
			if(Physics.Raycast(directionRay, out dirHit, 1.5f, projectionMask)) {
				direction = Vector3.ProjectOnPlane(direction, dirHit.normal).normalized;
			}
			lookDir = direction.normalized;
			if(direction.sqrMagnitude > 0.01f) { // mimimi use an epsilon
			}


			//Animation
			animator.SetFloat("PlayerSpeed", inputDirection.magnitude);
			onMove?.Invoke(transform.position);
			
			//Dashing
			if(isDashing && direction == Vector3.zero && !dashingByKeyboard) { //Dashing when standing still with mouse
				rb.velocity += dashMouseDir * dashAcceleration;
				rb.velocity = dashMouseDir * Mathf.Min(rb.velocity.magnitude, currentSpeed);
			}
			else if(isDashing && direction != Vector3.zero || dashingByKeyboard) {
				dashingByKeyboard = true;
				rb.velocity += dashDirection * dashAcceleration;
				rb.velocity = dashDirection * Mathf.Min(rb.velocity.magnitude, currentSpeed);
			}
			else
				rb.velocity += direction * locomotionSettings.Acceleration;

			//Speed control / decceleration
			if(direction != Vector3.zero && Mathf.RoundToInt(directionDelta) != -1)
				rb.velocity = direction * Mathf.Min(rb.velocity.magnitude, currentSpeed);
			else if(Mathf.RoundToInt(directionDelta) == -1)
				rb.velocity = rb.velocity * locomotionSettings.RapidDecceleration;
			else {
				Debug.Log("Deccelerating");
				rb.velocity = rb.velocity * locomotionSettings.Decceleration;
			}
				
			if(rb.velocity.y > 0)
				rb.velocity = new Vector3(rb.velocity.x, -1, rb.velocity.z); 
		}

		private void Rotate()
		{
			if(isAttacking) {
				return;
			}
			//Rotation by mouse
			Vector2 mousePos = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
			Vector3 worldMousePosition = new Vector3();
			Plane plane = new Plane(Vector3.up, Vector3.zero);

			Ray mouseRay = camera.ScreenPointToRay(mousePos);
			plane.Raycast(mouseRay, out float distance);
			var endPoint = mouseRay.origin + (mouseRay.direction * distance);
			worldMousePosition = endPoint;
			mouseDir = worldMousePosition - transform.position;
			mouseDir.Normalize();
			float angle = Mathf.Atan2(mouseDir.x, mouseDir.z) * Mathf.Rad2Deg;
			var previousRotation = transform.forward;
			transform.rotation = mouseDir != Vector3.zero
				? Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), .25f)
				: transform.rotation;
			
			var difference = Vector3.SignedAngle(previousRotation, transform.forward, Vector3.up);
			var direction = difference > 1.0f ? 1.0f : difference < -1.0f ? -1.0f : 0.0f;
			var dir = difference > 0.0f ? 1.0f : difference < -0.0f ? -1.0f : 0.0f;
			//Debug.Log($"Angle: {difference}");
			var previousSpeed = animator.GetFloat("PlayerSpeed");
			animator.SetFloat("PlayerSpeed", previousSpeed + Mathf.Abs(direction));
			var animAngle = Vector3.SignedAngle(mouseDir, lookDir, Vector3.up);
			animator.SetFloat("angle", Mathf.Approximately(animAngle, 0.0f) ? 90.0f * dir : animAngle);

			//Rotation by movement
			//Vector2 dir = new Vector2(rb.velocity.x, rb.velocity.z);
		}

		public void Lunge(float force, float multiplier = 1)
		{
			if(direction == Vector3.zero)
				rb.AddForce(transform.forward * force * multiplier);
		}

		private void Attack()
		{
			isAttacking = true; 
			currentSpeed = locomotionSettings.AttackingSpeed;
		} 
		private void AttackStopped()
		{
			isAttacking = false; 
			
			if(isBuffed)
				currentSpeed = uASettings.BuffedSpeed;
			else if(isDebuffed)
				currentSpeed = uASettings.DebuffedSpeed; 
			else
				currentSpeed = locomotionSettings.Speed;
		}

		private void BuffPlayer()
		{
			isBuffed = true;
			currentSpeed = uASettings.BuffedSpeed; 
		}

		private void StopBuffPlayer()
		{
			isBuffed = false; 
			currentSpeed = locomotionSettings.Speed; 
		}

		private void DebuffPlayer(float f)
		{
			if(isDebuffed)
				return; 
			
			Debug.Log("Debuff player");
			
			isDebuffed = true;
			currentSpeed = uASettings.DebuffedSpeed; 
		}

		private void StopDebuffPlayer(float f)
		{
			if(!isDebuffed)
				return;
			
			Debug.Log("Stop Debuff player");
			
			isDebuffed = false;
			currentSpeed = locomotionSettings.Speed;
		}
		

		public void AssignDashValues(float dashSpeed, float dAccel)
		{
			currentSpeed = dashSpeed;
			dashAcceleration = dAccel;
			dashMouseDir = mouseDir.normalized;
			dashMouseDir.y = 0;
			dashDirection = direction.normalized;
			dashDirection.y = 0;
			isDashing = true;
			hasResetValues = false; 
		}

		private void AssignCleanValues()
		{
			hasResetValues = false; 
			currentSpeed = locomotionSettings.CleaningSpeed;
		}

		private void AssignHealValues()
		{
			hasResetValues = false; 
			currentSpeed = locomotionSettings.HealingSpeed; 
		} 

		public void ResetValues()
		{
			if(hasResetValues)
				return;
			
			hasResetValues = true; 
			currentSpeed = isDebuffed ? uASettings.DebuffedSpeed : locomotionSettings.Speed;
			dashAcceleration = locomotionSettings.Acceleration;
			isDashing = false;
			dashingByKeyboard = false;
			hasDashed = true; 
		}

	

		private void Die()
		{
			rb.velocity = Vector3.zero; 
			this.enabled = false; 
		}
		
		private void OnEnable()
		{
			//Input
			controller.Activate();

			//Events
			events.onAttack.onEvent += Attack;
			events.onAttackStopped.onEvent += AttackStopped;
			onPlayerBuffed.onEvent += BuffPlayer;
			onPlayerBuffEnded.onEvent += StopBuffPlayer;
			onDl5.onEvent += DebuffPlayer;
			onDl4.onEvent += StopDebuffPlayer;
			onPlayerDeath.onEvent += Die;
			onClean.onEvent += AssignCleanValues;
			onCleanEnd.onEvent += ResetValues;
			onHeal.onEvent += AssignHealValues;
			onHealEnd.onEvent += ResetValues;
			attackStoppedFailCheck.onEvent += AttackStopped; 

			//Components
			rb = GetComponent<Rigidbody>();
			animator = GetComponentInChildren<Animator>();
			camera = GameObject.Find("Main Camera").GetComponent<Camera>();
			cameraTransform = camera.transform;
		}

		private void OnDisable()
		{
			events.onAttack.onEvent -= Attack;
			events.onAttackStopped.onEvent -= AttackStopped;
			onPlayerBuffed.onEvent -= BuffPlayer;
			onPlayerBuffEnded.onEvent -= StopBuffPlayer;
			onDl5.onEvent -= DebuffPlayer;
			onDl4.onEvent -= StopDebuffPlayer; 
			onPlayerDeath.onEvent -= Die;
			onClean.onEvent -= AssignCleanValues;
			onCleanEnd.onEvent -= ResetValues; 
			onHeal.onEvent -= AssignHealValues;
			onHealEnd.onEvent -= ResetValues; 
			attackStoppedFailCheck.onEvent -= AttackStopped; 
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(directionRay);
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, (Vector3.down * 1.7f));
		}
#endif
	}
}
