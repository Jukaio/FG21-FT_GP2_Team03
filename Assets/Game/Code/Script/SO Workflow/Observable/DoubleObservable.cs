using UnityEngine;

[CreateAssetMenu(fileName = "Double Observable", menuName = "Scriptable Objects/Gameplay/Observables/Double Observable")]
public class DoubleObservable : GenericObservable<double>
{
	[SerializeField]
	private DoubleReference valueReference;
	protected override GenericReference<double> ValueReference => valueReference;
}

