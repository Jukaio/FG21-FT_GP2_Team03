using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraTemp : MonoBehaviour
{
	[SerializeField] private Transform player;

	private void FixedUpdate()
	{
		transform.LookAt(player.position);
	}
}
