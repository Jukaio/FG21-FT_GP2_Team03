using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	[SerializeField]
	private RuntimeGameObjectReference target;

	private void LateUpdate()
	{
		transform.LookAt(target.value.transform);
	}
}
