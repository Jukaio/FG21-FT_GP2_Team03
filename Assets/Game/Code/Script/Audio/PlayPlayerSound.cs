using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPlayerSound : MonoBehaviour
{
    #region References
    private AudioManager audioManager;

    // IENUMERATOR stuff
    private bool ienumeratorRunning;
    private bool isCurrentlyPlaying = false;
    private float clipLength = 0;
    private bool hasPlayed = false;

    //PLAYER 
    [SerializeField] private VoidEvent onPlayerAttack; //swing
    [SerializeField] private IntEvent onSwordHit; //hit
    [SerializeField] private VoidEvent onClean; //cleaning
    [SerializeField] private Game.Controller controller; //heal , controller.onHeal
    [SerializeField] private VoidEvent onDash; //dash
    [SerializeField] private VoidEvent onPlayerDeath; //dying

    //ENEMY
    //enemy attack
    //enemy death
    
    //audioclips

    //ENVIRONMENT
    [SerializeField] private VoidEvent crystalBreak; //crystal break

    #endregion

    
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("PlayerAudioManager").GetComponent<AudioManager>();
    }

    #region onEnableDisable
    void OnEnable()
    {
        //PLAYER

        onPlayerAttack.onEvent += AttackSound;
        onSwordHit.onEvent += HitSound;
        //onClean.onEvent += CleanSound;
        controller.onHeal += HealSound;
        onDash.onEvent += DashAudio;
        onPlayerDeath.onEvent += DeathSound;


        //ENEMY

        //ENVIRONMENT

        crystalBreak.onEvent += CrystalSound;

    }
    void OnDisable()
    {
        //PLAYER
        
        onPlayerAttack.onEvent -= AttackSound;
        onSwordHit.onEvent -= HitSound;
        //onClean.onEvent -= StopAudio;
        //controller.onHeal -= StopHealAudio;
        controller.onHeal -= HealSound;
        onDash.onEvent -= DashAudio;
        onPlayerDeath.onEvent -= DeathSound;
        
        //ENEMY

        //ENVIRONMENT

        crystalBreak.onEvent -= CrystalSound;

    }
    #endregion

    #region PlaySoundFunctions
    
    public void StopAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Pause();
    }
    public void AttackSound()
    {
        if (isCurrentlyPlaying == false && ienumeratorRunning ==false)
        {
        //int r = Random.Range(0, audioManager.swordSwing.Length);
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioManager.swordSwing[0].clip;
        audio.Play();
        isCurrentlyPlaying = true;
        clipLength = audio.clip.length;
        StartCoroutine(AttackPlaying(clipLength));
        }
    }
    public void HitSound(int _)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioManager.playerActions[0].clip;
        audio.Play();
    }
    public void HealSound(Game.ButtonState state)
    {   
        AudioSource audio = GetComponent<AudioSource>();
        if(hasPlayed ==false)
        {
        audio.clip = audioManager.playerActions[1].clip;
        audio.Play();
        clipLength = audio.clip.length;
        hasPlayed=true;
        }
        else
        {
            audio.Pause();
            hasPlayed = false;
        }
    }
    public void StopHealAudio(Game.ButtonState state)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Pause();
        Debug.Log("stopaudio");
    }
    public void CleanAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioManager.playerActions[2].clip;
        audio.Play();
    }
    public void DashAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioManager.playerActions[3].clip;
        audio.Play();
    }
    public void CrystalSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioManager.environmentSounds[2].clip;
        audio.Play();
    }
    public void DeathSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioManager.deathSounds[0].clip;
        audio.Play();
    }
    #endregion

    IEnumerator AttackPlaying(float clipLength)
    {
        ienumeratorRunning = true;
        Debug.Log(clipLength);
        yield return new WaitForSeconds(clipLength);
        isCurrentlyPlaying = false;
        ienumeratorRunning = false;
    }
}
