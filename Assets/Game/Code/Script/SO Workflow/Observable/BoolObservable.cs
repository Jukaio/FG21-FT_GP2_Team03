using UnityEngine;

[CreateAssetMenu(fileName = "Bool Observable", menuName = "Scriptable Objects/Gameplay/Observables/Bool Observable")]
public class BoolObservable : GenericObservable<bool>
{
	[SerializeField]
	private BoolReference valueReference;
	protected override GenericReference<bool> ValueReference => valueReference;
}

