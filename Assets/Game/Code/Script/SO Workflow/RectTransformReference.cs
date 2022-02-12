using UnityEngine;
#if UNITY_EDITOR
#endif

[CreateAssetMenu(fileName = "Rect Transform Reference", menuName = "Scriptable Objects/Rect Transform Reference")]
public class RectTransformReference : ScriptableObject
{
	public RectTransform Value
	{
		get;
		set;
	}
}

