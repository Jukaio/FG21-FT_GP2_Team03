using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
using System.Linq;
#endif
using Utility;

namespace Game
{
	// Health and Mana
	[CreateAssetMenu(fileName = "Bucket", menuName = "Scriptable Objects/Gameplay/Bucket/Bucket")]
	public class Bucket : ScriptableObject
	{
		[SerializeField]
		private FloatObservable currentFill;
		[SerializeField]
		private FloatObservable maximumFill;
		[SerializeField]
		private FloatReference startingFill;
		[SerializeField]
		private FloatEvent onEmptyFill;
		[SerializeField]
		private FloatEvent onFullFill;

		[SerializeField]
		private SpecifiedFillingEventCollection onSpecifiedFillingCollection;

		public event System.Action<float> OnCurrentChanged
		{
			add => currentFill.onChange += value;
			remove => currentFill.onChange -= value;
		}
		public event System.Action<float> OnMaximumChanged
		{
			add => maximumFill.onChange += value;
			remove => maximumFill.onChange -= value;
		}

		public float RestToFill => Maximum - Current;
		public bool IsFull => Current >= Maximum;
		public bool IsEmpty => Current <= 0.0f;

		public bool WouldBeTooFull(float rate)
		{
			return (Current + rate) > Maximum;
		}

		public bool WouldBeTooEmpty(float rate)
		{
			return (Current + rate) < 0.0f;
		}

		public float Current
		{
			get => currentFill.Value;
			set
			{
				currentFill.Value = Mathf.Clamp(value, 0, Maximum);
				if(Current <= 0) {
					if(onEmptyFill != null) {
						onEmptyFill.Invoke(Current);
					}
				}
				else if(Current >= Maximum) {
					if(onFullFill != null) {
						onFullFill.Invoke(Current);
					}
				}

				if(onSpecifiedFillingCollection != null && onSpecifiedFillingCollection.Events.Count > 0) {
					float ratio = (float)(Current) / (float)(Maximum);
					for(int i = 0; i < onSpecifiedFillingCollection.Events.Count; i++) {
						var that = onSpecifiedFillingCollection.Events[i];
						if(that != null) {
							if(ratio >= that.Min && ratio < that.Max) {
								that.Invoke(Current);
							}
						}
					}
				}
			}
		}
		public float Maximum
		{
			get => maximumFill.Value;
			set => maximumFill.Value = value;

		}
		public float Starting => startingFill.Value;

		void OnEnable()
		{
			if(currentFill != null && startingFill != null) {
				currentFill.Value = startingFill.Value;
			}
		}
	}

#if UNITY_EDITOR
	//[CustomEditor(typeof(Bucket))]
	//public class BucketInspector : NicelyDrawnEditor
	//{

	//}
#endif
}

