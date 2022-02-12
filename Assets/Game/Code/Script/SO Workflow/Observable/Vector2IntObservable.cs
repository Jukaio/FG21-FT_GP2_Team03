using UnityEngine;

[CreateAssetMenu(fileName = "Vector2Int Observable", menuName = "Scriptable Objects/Gameplay/Observables/Vector2Int Observable")]
public class Vector2IntObservable : GenericObservable<Vector2Int>
{
	[SerializeField]
	private Vector2IntReference valueReference;
	protected override GenericReference<Vector2Int> ValueReference => valueReference;
}

