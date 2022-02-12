using UnityEngine;

[CreateAssetMenu(fileName = "Float Observable", menuName = "Scriptable Objects/Gameplay/Observables/Float Observable")]
public class FloatObservable : GenericObservable<float>
{
	[SerializeField]
	private FloatReference valueReference;
	protected override GenericReference<float> ValueReference => valueReference;

	public void Initialise()
	{
		if(valueReference == null) {
			valueReference = CreateInstance<FloatReference>();
		}
	}
}

