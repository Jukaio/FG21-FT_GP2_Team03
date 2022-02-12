using UnityEngine;

public abstract class Vector3Observer : GenericObserver<Vector3>
{
	[SerializeField]
	protected Vector3Observable vector3Observable;
	protected override GenericObservable<Vector3> Observable => vector3Observable;
}
