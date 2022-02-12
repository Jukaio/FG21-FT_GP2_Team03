using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketUI : FloatObserver
{
	[SerializeField]
	private FloatReference maximumReference;

	public float Maximum => maximumReference.Value;

	private UnityEngine.UI.Image image;

	private void Awake()
	{
		image = GetComponent<UnityEngine.UI.Image>();
	}


	protected override void OnChange(float value)
	{
		var fillAmount = value / maximumReference.Value;
		image.fillAmount = fillAmount;
	}

	public void SetMaxiumumHealthReference(FloatReference floatReference)
	{
		maximumReference = floatReference;
	}

	public void SetCurrentHealthReference(FloatObservable floatObservable)
	{
		this.floatObservable = floatObservable;
	}
}
