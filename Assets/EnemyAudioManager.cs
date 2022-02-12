using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
	public Sound[] audio; 
	
	
	[System.Serializable]
	public class Sound
	{
		public string name;
		public AudioClip[] clip; 
	}
}
