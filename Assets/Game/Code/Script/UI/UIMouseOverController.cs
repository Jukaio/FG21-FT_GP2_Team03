using System;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.InputSystem; 

public class UIMouseOverController : MonoBehaviour
{
	[Header("Components")] 
	[SerializeField] private Texture[] mouseIcons; 
	
	[Header("Events")]
	[SerializeField] private IntEvent onMouseEnter; 
	[SerializeField] private VoidEvent onMouseExit;

	private Transform mouseIconTransform; 
	private RawImage mouseIcon;

	private void EnableCursor() => Cursor.visible = true;
	private void DisableCursor() => Cursor.visible = false;

	private void Update()
	{
		mouseIconTransform.position = Mouse.current.position.ReadValue();
	}

	private void ChangeIcon(int n)
	{
		Debug.Log("Changing Icon");
		mouseIcon.texture = mouseIcons[n];
		mouseIcon.enabled = true;
		DisableCursor(); 
	}

	private void HideIcon()
	{
		Debug.Log("Hiding Icon");
		mouseIcon.enabled = false;
		EnableCursor(); 
	}
	
	
	private void OnEnable()
	{
		mouseIconTransform = GameObject.Find("MouseIcon").transform;
		mouseIcon = mouseIconTransform.GetComponent<RawImage>();
		
		onMouseEnter.onEvent += ChangeIcon;
		onMouseExit.onEvent += HideIcon; 
	}
}
