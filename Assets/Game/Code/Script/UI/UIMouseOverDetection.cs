using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI; 
using Game;
using Debug = UnityEngine.Debug;

namespace UI
{
	public class UIMouseOverDetection : MonoBehaviour
	{
		[Header("Settings")] 
		[SerializeField] private Vector2 mouseIconOffset; 
		
		[Header("Components")] 
		[SerializeField] private RawImage mouseIcon; 
		[SerializeField] private Game.Controller controller; 
		[SerializeField] private RectTransform swordUITransform;

		private void Update()
		{
			CheckIfMouseOver();
			FollowMouse(); 
		}

		private void FollowMouse()
		{
			mouseIcon.transform.position = controller.Target + mouseIconOffset; 
		}

		private void CheckIfMouseOver()
		{
			var local = swordUITransform.InverseTransformPoint(controller.Target);
			if(swordUITransform.rect.Contains(local))
				mouseIcon.enabled = true;
			else
				mouseIcon.enabled = false;
		}
	}
}
