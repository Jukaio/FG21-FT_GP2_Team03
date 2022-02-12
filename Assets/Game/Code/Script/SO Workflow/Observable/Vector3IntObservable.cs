using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Int Observable", menuName = "Scriptable Objects/Gameplay/Observables/Vector3Int Observable")]
public class Vector3IntObservable : GenericObservable<Vector3Int>
{
	[SerializeField]
	private Vector3IntReference valueReference;
	protected override GenericReference<Vector3Int> ValueReference => valueReference;
}

