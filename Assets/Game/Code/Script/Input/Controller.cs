using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.PlayerLoop;
using UnityEngine.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
	public enum ButtonState
	{
		Pressed,
		Released
	}

	[CreateAssetMenu(fileName = "Controls", menuName = "Scriptable Objects/Global/Controller")]
	public class Controller : ScriptableObject
	{
		[SerializeField]
		private InputActionAsset inputAsset;
		static private PlayerInput instance = null;

		public Vector2 Move
		{
			get; private set;
		}
		public Vector2 Look
		{
			get; private set;
		}
		public Vector2 Target
		{
			get; private set;
		}
		public ButtonState Attack
		{
			get; private set;
		}
		public ButtonState Dodge
		{
			get; private set;
		}
		public ButtonState Clean
		{
			get; private set;
		}
		public ButtonState Heal
		{
			get; private set;
		}
		public string DeviceName
		{
			get; private set;
		}

		const string moveLabel = "Move";
		public delegate void Vector2Action(Vector2 delta);
		public event Vector2Action onMove;

		const string lookLabel = "Look";
		public event Vector2Action onLook;

		const string targetLabel = "Target";
		public event Vector2Action onTarget;

		const string attackLabel = "Attack";
		public delegate void ButtonAction(ButtonState state);
		public event ButtonAction onAttack;

		const string cleanLabel = "Clean";
		public event ButtonAction onClean;

		const string dodgeLabel = "Dodge";
		public event ButtonAction onDodge;

		const string healLabel = "Heal";
		public event ButtonAction onHeal;

		public delegate void OnDeviceChangeAction(string deviceName);
		public event OnDeviceChangeAction onDeviceChange;

		private Dictionary<Object, Coroutine> routinesBlockAll = new Dictionary<Object, Coroutine>();
		private List<InputAction> inputActions = new List<InputAction>();

		public void Activate()
		{
			if(instance == null) {
				Construct();
				AssignAndActivateActions();
			}

			instance.enabled = true;
		}

		public void Block(Object caller, float timeInSeconds)
		{
			if(!routinesBlockAll.ContainsKey(caller)) {
				routinesBlockAll.Add(caller, instance.StartCoroutine(Blocking(caller, timeInSeconds)));
			}
		}

		/// <summary>
		/// Blocks the input until the supplied delegate evaluates to false.
		/// </summary>
		/// <param name="func"></param>
		public void BlockWhile(Object caller, System.Func<bool> func)
		{
			if(!routinesBlockAll.ContainsKey(caller)) {
				routinesBlockAll.Add(caller, instance.StartCoroutine(BlockingWhile(caller, func)));
			}
		}

		/// <summary>
		/// Blocks the input until the supplied delegate evaluates to true.
		/// </summary>
		/// <param name="func"></param>
		public void BlockUntil(Object caller, System.Func<bool> func)
		{
			if(!routinesBlockAll.ContainsKey(caller)) {
				routinesBlockAll.Add(caller, instance.StartCoroutine(BlockingUntil(caller, func)));
			}
		}

		private void Construct()
		{
			var dontDestroyOnLoad = new GameObject("Input");
			instance = dontDestroyOnLoad.AddComponent<PlayerInput>();
			instance.actions = inputAsset;
			instance.SwitchCurrentActionMap("Player");
			instance.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
			DontDestroyOnLoad(dontDestroyOnLoad);
		}

		private void AssignAndActivateActions()
		{
			instance.onControlsChanged += OnDeviceChange;

			AssignActivateAction(instance.actions[moveLabel], OnMove);
			AssignActivateAction(instance.actions[lookLabel], OnLook);
			AssignActivateAction(instance.actions[targetLabel], OnTarget);
			AssignActivateAction(instance.actions[attackLabel], OnAttack);
			AssignActivateAction(instance.actions[dodgeLabel], OnDodge);
			AssignActivateAction(instance.actions[cleanLabel], OnClean);
			AssignActivateAction(instance.actions[healLabel], OnHeal);
		}

		private void AssignActivateAction(InputAction inputAction, System.Action<InputAction.CallbackContext> callback)
		{
			inputAction.performed += callback;
			inputAction.canceled += callback;
			inputAction.Enable();
			inputActions.Add(inputAction);
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			Move = context.ReadValue<Vector2>();
			onMove?.Invoke(Move);
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			Look = context.ReadValue<Vector2>();
			onLook?.Invoke(Look);
		}

		public void OnTarget(InputAction.CallbackContext context)
		{
			Target = context.ReadValue<Vector2>();
			onTarget?.Invoke(Target);
		}

		private void OnAttack(InputAction.CallbackContext context)
		{
			Attack = context.phase == InputActionPhase.Performed ?
					ButtonState.Pressed :
					ButtonState.Released;
			onAttack?.Invoke(Attack);
		}

		private void OnClean(InputAction.CallbackContext context)
		{
			Clean = context.phase == InputActionPhase.Performed ?
					ButtonState.Pressed :
					ButtonState.Released;
			onClean?.Invoke(Clean);
		}

		private void OnHeal(InputAction.CallbackContext context)
		{
			Heal = context.phase == InputActionPhase.Performed ?
					ButtonState.Pressed :
					ButtonState.Released;
			onHeal?.Invoke(Heal);
		}

		private void OnDodge(InputAction.CallbackContext context)
		{
			Dodge = context.phase == InputActionPhase.Performed ?
					ButtonState.Pressed :
					ButtonState.Released;
			onDodge?.Invoke(Dodge);
		}

		private void OnDeviceChange(PlayerInput input)
		{
			DeviceName = input.currentControlScheme;
			onDeviceChange?.Invoke(DeviceName);
		}

		private IEnumerator Blocking(Object caller, float time)
		{
			Block();
			yield return new WaitForSeconds(time);
			RemoveCallerAndTryUnblocking(caller);
		}
		private IEnumerator BlockingWhile(Object caller, System.Func<bool> func)
		{
			Block();
			yield return new WaitWhile(func);
			RemoveCallerAndTryUnblocking(caller);
		}

		private IEnumerator BlockingUntil(Object caller, System.Func<bool> func)
		{
			Block();
			yield return new WaitUntil(func);
			RemoveCallerAndTryUnblocking(caller);
		}

		private void RemoveCallerAndTryUnblocking(Object caller)
		{
			routinesBlockAll.Remove(caller);
			if(routinesBlockAll.Count == 0) {
				Unblock();
			}
		}

		private void Unblock()
		{
			foreach(var action in inputActions) {
				action.Enable();
			}
		}

		private void Block()
		{
			foreach(var action in inputActions) {
				action.Disable();
			}
		}
	}
}
