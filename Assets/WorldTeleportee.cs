using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTeleportee : MonoBehaviour
{
	[SerializeField]
	private Vector3Event onTeleport;

	public Vector3Event OnTeleport => onTeleport;
}
