using System;
using UnityEngine;
using Game;
using UnityEngineInternal;

public class UIMouseOverButton : MonoBehaviour
{
	[SerializeField] private VoidEvent onMouseOverButton; 
	[SerializeField] private Game.Controller controller;
	
	private RectTransform rTransform;
	

	private bool mouseOver; 

	private void Update()
	{
		CheckIfMouseOver();
	}

	private void CheckIfMouseOver()
	{
		var local = rTransform.InverseTransformPoint(controller.Target);
		if(rTransform.rect.Contains(local) && !mouseOver) {
			mouseOver = true;
			Debug.Log("Mouse Over");
			onMouseOverButton.Invoke(); 
		}
		else 
			mouseOver = false;
	}

	private void OnEnable()
	{
		rTransform = GetComponent<RectTransform>();
	}
}
