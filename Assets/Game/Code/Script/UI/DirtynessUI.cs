using System;
using UnityEngine;
using UnityEngine.UI; 

public class DirtynessUI : MonoBehaviour
{
	[SerializeField] private FloatEvent onDl5; 
	[SerializeField] private FloatEvent onDl4; 
	[SerializeField] private FloatObservable dirtObservable;
	[SerializeField] private Image swordBorder; 
	
	private Image dirtynessUI;

	private bool isDirty;
	private float index;
	private float omegaY = 2.5f;
	
	private void Update()
	{
		if(isDirty)
			BlinkSwordBorder();
	}

	private void ChangeUIAlpha(float amount)
	{
		print("OnChange");
		
		amount /= 100;
		Color tempColor = dirtynessUI.color;
		tempColor.a = amount;
		dirtynessUI.color = tempColor; 
	}

	private void BlinkSwordBorder()
	{
		Debug.Log("Sword Blinking");
		Color tempColor = swordBorder.color;
		
		index += Time.deltaTime;
		tempColor.a = Mathf.Abs (Mathf.Sin (omegaY*index));
		
		swordBorder.color = tempColor; 
	}

	private void OnDirtLevel4(float f)
	{
		isDirty = false;
		Color tempColor = swordBorder.color;
		tempColor.a = 0;
		swordBorder.color = tempColor; 
	}
	
	private void OnEnable()
	{
		dirtynessUI = GetComponent<Image>();
		dirtObservable.onChange += ChangeUIAlpha;
		onDl5.onEvent += (float f) => isDirty = true;
		onDl4.onEvent += OnDirtLevel4; 
	}

	private void OnDisable()
	{	
		dirtObservable.onChange -= ChangeUIAlpha;
	}
}
