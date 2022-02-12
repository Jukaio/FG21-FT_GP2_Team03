using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Vector3Int Reference", menuName = "Scriptable Objects/Gameplay/Reference/Vector3Int Reference")]
public class Vector3IntReference : GenericReference<Vector3Int>
{

}

#if UNITY_EDITOR
[CustomEditor(typeof(Vector3IntReference))]
public class Vector3IntReferenceInspector : Vector3IntReference.Inspector
{
	public override Vector3Int DrawField(string label, Vector3Int current)
	{
		return EditorGUILayout.Vector3IntField(label, current);
	}
}
#endif
