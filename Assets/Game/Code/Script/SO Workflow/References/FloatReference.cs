using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Float Reference", menuName = "Scriptable Objects/Gameplay/Reference/Float Reference")]
public class FloatReference : GenericReference<float>
{

}

#if UNITY_EDITOR
[CustomEditor(typeof(FloatReference))]
public class FloatReferenceInspector : FloatReference.Inspector
{
	public override float DrawField(string label, float current)
	{
		return EditorGUILayout.FloatField(label, current);
	}
}
#endif

