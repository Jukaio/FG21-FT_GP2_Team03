using UnityEngine;

namespace Levels
{
	public class WorldDoor : MonoBehaviour
	{
		public Vector3 teleportPoint = Vector3.zero;
		public bool doorIsOpen = true;
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(teleportPoint, 1.0f);
		}

		private void OnCollisionEnter(Collision collision)
		{
			if(doorIsOpen) 
			{
				if(collision.gameObject.TryGetComponent<WorldTeleportee>(out var teleporteee)) {
					var y = collision.gameObject.transform.position.y;
					//collision.gameObject.transform.position = new Vector3(teleportPoint.x, y, teleportPoint.z);
					teleporteee.OnTeleport?.Invoke(new Vector3(teleportPoint.x, y, teleportPoint.z));
				}
			}
			
		}
		public void OpenDoors()
		{
			doorIsOpen = true;
			//ParticleSystem.Play();
		}

		public void CloseDoors()
		{
			Debug.Log("Closing");
			doorIsOpen = false;
			//ParticleSystem.Stop();
		}
	}
}

