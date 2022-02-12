using UnityEngine;
using System.Collections; 

public class PlayAmbiance : MonoBehaviour
{
	[SerializeField] private VoidEvent onBossRoomEntered;
	[SerializeField] private Vector3Event onTeleport;
	[SerializeField] private AudioSource ambienceSfxSource;
	
	
	private AmbianceManager ambianceManager;
	private AudioSource audio; 
	
    
    void OnEnable()
    {
	    ambianceManager = GameObject.FindGameObjectWithTag("AmbianceAudioManager").GetComponent<AmbianceManager>();
	    audio = GetComponent<AudioSource>();

	    onTeleport.onEvent += OnDoorTransition; 

	    //onBossRoomEntered.onEvent += PlayBossMusic;
    }
    void OnDisable()
    {
	    //onBossRoomEntered.onEvent -= PlayBossMusic;
    }
    void Start()
    {
	    audio.clip = ambianceManager.ambiance[0].clip;
        audio.Play();
    }
    private void PlayBossMusic()
    {
	    audio.clip = ambianceManager.ambiance[1].clip;
	    audio.Play(); 
    }

    private void OnDoorTransition(Vector3 v3) => StartCoroutine(PlayDoorTransitionSfx());

    private IEnumerator PlayDoorTransitionSfx()
    {
	    ambienceSfxSource.clip = ambianceManager.ambiance[2].clip;
	    ambienceSfxSource.Play(); 
	    yield return new WaitForSeconds(ambienceSfxSource.clip.length);
	    ambienceSfxSource.clip = ambianceManager.ambiance[3].clip;
	    ambienceSfxSource.Play(); 
    }
}
