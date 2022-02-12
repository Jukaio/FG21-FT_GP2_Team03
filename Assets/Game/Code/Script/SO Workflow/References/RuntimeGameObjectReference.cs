using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObject Runtime Reference", menuName = "Scriptable Objects/Gameplay/Reference/GameObject Runtime Reference")]
public class RuntimeGameObjectReference : ScriptableObject
{
	[SerializeField]
	public GameObject value;
	public GameObject Value
	{
		get => this.value;
		set => this.value = value;
	}
}
