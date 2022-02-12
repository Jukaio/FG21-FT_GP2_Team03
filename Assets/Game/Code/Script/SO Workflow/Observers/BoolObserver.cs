using UnityEngine;

public abstract class BoolObserver : GenericObserver<bool>
{
	[SerializeField]
	protected BoolObservable boolObservable;
	protected override GenericObservable<bool> Observable => boolObservable;
}
