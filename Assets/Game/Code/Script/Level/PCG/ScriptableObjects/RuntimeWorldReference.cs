using UnityEngine;

namespace Levels
{
	[CreateAssetMenu(fileName = "Runtime World Reference", menuName = "Scriptable Objects/Global/Runtime World Reference")]
	public class RuntimeWorldReference : ScriptableObject
	{
		private World value;
		[SerializeField]
		private VoidEvent onWorldChange = null;
		public World Value
		{
			get => value;
			set
			{
				if(value != this.value) {
					this.value = value;
					onWorldChange?.Invoke();
				}
			}
		}
	}
}
