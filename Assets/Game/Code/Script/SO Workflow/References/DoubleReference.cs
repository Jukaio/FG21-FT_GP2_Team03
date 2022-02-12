using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Double Reference", menuName = "Scriptable Objects/Gameplay/Reference/Double Reference")]
public class DoubleReference : GenericReference<double>
{

}

#if UNITY_EDITOR
[CustomEditor(typeof(DoubleReference))]
public class DoubleReferenceInspector : DoubleReference.Inspector
{
	public override double DrawField(string label, double current)
	{
		return EditorGUILayout.DoubleField(label, current);
	}
}
#endif

