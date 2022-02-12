using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFader : MonoBehaviour
{
	[SerializeField]
	private FloatEvent onFade;

	private UnityEngine.UI.Image image;

	private void Awake()
	{
		image = GetComponent<UnityEngine.UI.Image>();
	}

	private void OnEnable()
	{
		onFade.onEvent += OnFade;
	}

	private void OnDisable()
	{
		onFade.onEvent -= OnFade;
	}

	private void OnFade(float value)
	{
		image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.SmoothStep(0.0f, 1.0f, value));
	}
}
