using UnityEngine;

[CreateAssetMenu(fileName = "Vector2 Observable", menuName = "Scriptable Objects/Gameplay/Observables/Vector2 Observable")]
public class Vector2Observable : GenericObservable<Vector2>
{
	[SerializeField]
	private Vector2Reference valueReference;
	protected override GenericReference<Vector2> ValueReference => valueReference;
}

