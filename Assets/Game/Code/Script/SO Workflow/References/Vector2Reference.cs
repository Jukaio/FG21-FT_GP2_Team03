using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Vector2 Reference", menuName = "Scriptable Objects/Gameplay/Reference/Vector2 Reference")]
public class Vector2Reference : GenericReference<Vector2>
{

}

#if UNITY_EDITOR
[CustomEditor(typeof(Vector2Reference))]
public class Vector2ReferenceInspector : Vector2Reference.Inspector
{
	public override Vector2 DrawField(string label, Vector2 current)
	{
		return EditorGUILayout.Vector2Field(label, current);
	}
}
#endif
