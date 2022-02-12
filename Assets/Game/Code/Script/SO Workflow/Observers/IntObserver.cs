using UnityEngine;

public abstract class IntObserver : GenericObserver<int>
{
	[SerializeField]
	protected IntObservable intObservable;
	protected override GenericObservable<int> Observable => intObservable;
}
