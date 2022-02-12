using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Bool Reference", menuName = "Scriptable Objects/Gameplay/Reference/Bool Reference")]
public class BoolReference : GenericReference<bool>
{

}

#if UNITY_EDITOR
[CustomEditor(typeof(BoolReference))]
public class BoolReferenceInspector : BoolReference.Inspector
{
	public override bool DrawField(string label, bool current)
	{
		return EditorGUILayout.Toggle(label, current);
	}
}
#endif

