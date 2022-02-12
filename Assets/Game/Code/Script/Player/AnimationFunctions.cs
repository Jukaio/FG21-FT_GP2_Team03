using UnityEngine;
using Player; 

public class AnimationFunctions : MonoBehaviour
{
	[SerializeField] private SwordEvents events;

	public void OnAttackAnimationStopped() => events.onAttackStopped.Invoke();
}
