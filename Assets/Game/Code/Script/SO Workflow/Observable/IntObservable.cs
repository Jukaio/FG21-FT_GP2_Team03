using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Int Observable", menuName = "Scriptable Objects/Gameplay/Observables/Int Observable")]
public class IntObservable : GenericObservable<int>
{
	[SerializeField]
	private IntReference valueReference;
	protected override GenericReference<int> ValueReference => valueReference;
}


