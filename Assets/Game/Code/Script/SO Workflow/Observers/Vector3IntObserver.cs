using UnityEngine;

public abstract class Vector3IntObserver : GenericObserver<Vector3Int>
{
	[SerializeField]
	protected Vector3IntObservable vector3IntObservable;
	protected override GenericObservable<Vector3Int> Observable => vector3IntObservable;
}
