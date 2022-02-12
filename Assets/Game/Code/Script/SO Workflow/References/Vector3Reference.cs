using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Vector3 Reference", menuName = "Scriptable Objects/Gameplay/Reference/Vector3 Reference")]
public class Vector3Reference : GenericReference<Vector3>
{

}


#if UNITY_EDITOR
[CustomEditor(typeof(Vector3Reference))]
public class Vector3ReferenceInspector : Vector3Reference.Inspector
{
	public override Vector3 DrawField(string label, Vector3 current)
	{
		return EditorGUILayout.Vector3Field(label, current);
	}
}
#endif

