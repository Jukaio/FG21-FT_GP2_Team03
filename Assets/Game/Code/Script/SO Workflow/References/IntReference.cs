using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Int Reference", menuName = "Scriptable Objects/Gameplay/Reference/Int Reference")]
public class IntReference : GenericReference<int>
{

}

#if UNITY_EDITOR
[CustomEditor(typeof(IntReference))]
public class IntReferenceInspector : IntReference.Inspector
{
	public override int DrawField(string label, int current)
	{
		return EditorGUILayout.IntField(label, current);
	}
}
#endif

