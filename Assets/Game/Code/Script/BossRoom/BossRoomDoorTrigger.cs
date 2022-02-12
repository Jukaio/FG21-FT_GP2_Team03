using UnityEngine;

public class BossRoomDoorTrigger : MonoBehaviour
{
	[SerializeField] private VoidEvent onTriggerEvent;

	private static bool hasEnteredBossRoom;
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player") {
			
			if(!hasEnteredBossRoom)
				onTriggerEvent.Invoke();
			
			BossRoomDoorTrigger.hasEnteredBossRoom = true;
		}
	}
}
