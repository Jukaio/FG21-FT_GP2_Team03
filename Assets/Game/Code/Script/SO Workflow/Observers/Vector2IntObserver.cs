using UnityEngine;

public abstract class Vector2IntObserver : GenericObserver<Vector2Int>
{
	[SerializeField]
	protected Vector2IntObservable vector2IntObservable;
	protected override GenericObservable<Vector2Int> Observable => vector2IntObservable;
}
