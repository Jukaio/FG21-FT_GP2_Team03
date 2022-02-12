using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeGameObjectReferenceSetter : MonoBehaviour
{
	[SerializeField]
	private RuntimeGameObjectReference gameObjectReference;

	void Start()
    {
		gameObjectReference.value = gameObject;
    }

}
