using UnityEngine;

public abstract class DoubleObserver : GenericObserver<double>
{
	[SerializeField]
	protected DoubleObservable doubleObservable;
	protected override GenericObservable<double> Observable => doubleObservable;
}
