using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Vector2Int Reference", menuName = "Scriptable Objects/Gameplay/Reference/Vector2Int Reference")]
public class Vector2IntReference : GenericReference<Vector2Int>
{

}


#if UNITY_EDITOR
[CustomEditor(typeof(Vector2IntReference))]
public class Vector2IntReferenceInspector : Vector2IntReference.Inspector
{
	public override Vector2Int DrawField(string label, Vector2Int current)
	{
		return EditorGUILayout.Vector2IntField(label, current);
	}
}
#endif
