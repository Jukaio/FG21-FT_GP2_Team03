using UnityEngine;

public abstract class Vector2Observer : GenericObserver<Vector2>
{
	[SerializeField]
	protected Vector2Observable vector2Observable;
	protected override GenericObservable<Vector2> Observable => vector2Observable;
}
