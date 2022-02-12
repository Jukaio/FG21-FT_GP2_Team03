using UnityEngine;

public abstract class FloatObserver : GenericObserver<float>
{
	[SerializeField]
	protected FloatObservable floatObservable;
	protected override GenericObservable<float> Observable => floatObservable;
}
