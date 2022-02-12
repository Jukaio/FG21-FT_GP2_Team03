using UnityEngine;

public class RectTransformReferenceAssigner : MonoBehaviour
{
	[SerializeField]
	private RectTransformReference reference;

	private void Awake()
	{
		reference.Value = transform as RectTransform;	
	}
}

