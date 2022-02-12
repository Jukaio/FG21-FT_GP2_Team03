using UnityEngine;

[CreateAssetMenu(fileName = "Vector3 Observable", menuName = "Scriptable Objects/Gameplay/Observables/Vector3 Observable")]
public class Vector3Observable : GenericObservable<Vector3>
{
	[SerializeField]
	private Vector3Reference valueReference;
	protected override GenericReference<Vector3> ValueReference => valueReference;
}

