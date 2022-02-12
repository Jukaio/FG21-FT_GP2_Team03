using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayDialogue : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private string cc;
    private Text closedCap;
    private bool isCurrentlyPlaying;
    private float clipLength;

    //is the enumerator running?
    private bool isRunning;

    //event reference
    [SerializeField] private IntEvent onSwordHit;
    [SerializeField] private VoidEvent onClean;
    [SerializeField] private FloatEvent onSpecifiedFilling;

    //exposition
    [SerializeField] private VoidEvent expositionOne;
    [SerializeField] private VoidEvent expositionTwo;
    [SerializeField] private VoidEvent expositionThree;
    [SerializeField] private VoidEvent expositionFour;
    [SerializeField] private VoidEvent expositionFive;
    [SerializeField] private VoidEvent expositionSix;

	// Start is called before the first frame update
	void Start()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("DialogueAudioManager").GetComponent<DialogueManager>();
        closedCap = GameObject.FindGameObjectWithTag("CCText").GetComponent<Text>();
        //in other scripts, they should only need to reference PlayDialogue script and DialogueAttack(); or something similar to play the dialogue
        clipLength = 0;
        cc = " ";
        isCurrentlyPlaying = false;
        isRunning =false;
        CloseCaption(cc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        onSwordHit.onEvent += DialogueAttack;
        onClean.onEvent += DialogueCleaning;
        onSpecifiedFilling.onEvent += DialogueDirty;
        // .onEvent += DialogueRandom;
        expositionOne.onEvent += ExpositionOne;
        expositionTwo.onEvent += ExpositionTwo;
        expositionThree.onEvent += ExpositionThree;
        expositionFour.onEvent += ExpositionFour;
        expositionFive.onEvent += ExpositionFive;
    }
    void OnDisable()
    {
        onSwordHit.onEvent -= DialogueAttack;
        onClean.onEvent -= DialogueCleaning;
        onSpecifiedFilling.onEvent -= DialogueDirty;

        expositionOne.onEvent -= ExpositionOne;
        expositionTwo.onEvent -= ExpositionTwo;
        expositionThree.onEvent -= ExpositionThree;
        expositionFour.onEvent -= ExpositionFour;
        expositionFive.onEvent -= ExpositionFive;
    }

    IEnumerator Playing(float clipLength)
    {
        isRunning = true;
        Debug.Log(clipLength);
        yield return new WaitForSeconds(clipLength);
        isCurrentlyPlaying = false;
        Debug.Log("exited");
        cc = " ";
        CloseCaption(cc);
        yield return new WaitForSeconds (10);
        isRunning = false;
    }
    IEnumerator DirtyPlaying(float clipLength)
    {
        isRunning = true;
        Debug.Log(clipLength);
        yield return new WaitForSeconds(clipLength);
        isCurrentlyPlaying = false;
        Debug.Log("exited");
        cc = " ";
        CloseCaption(cc);
        isRunning = false;
    }
        
    public void DialogueAttack(int _)
    {
        if (isCurrentlyPlaying == false && isRunning ==false)
        {

            int a = Random.Range(0, dialogueManager.attackDialogue.Length);
            AudioSource audio = GetComponent<AudioSource>();

            audio.clip = dialogueManager.attackDialogue[a].clip;
            audio.Play();
            isCurrentlyPlaying = true;
     
            clipLength = audio.clip.length;

            cc = dialogueManager.attackDialogue[a].name;
            CloseCaption(cc);

            StartCoroutine(Playing(clipLength));
        }
        else if(isCurrentlyPlaying)
        {
            if(isRunning == false)
            {
                StartCoroutine(Playing(clipLength));
            }
        }
        
    }
    public void DialogueDirty(float _)
    {
        if (isCurrentlyPlaying == false && isRunning ==false)
        {
        int d = Random.Range(0, dialogueManager.dirtyDialogue.Length);
        AudioSource audio = GetComponent<AudioSource>();
        
        audio.clip = dialogueManager.dirtyDialogue[d].clip;
        audio.Play();
        isCurrentlyPlaying = true;

        clipLength = audio.clip.length;

        cc = dialogueManager.dirtyDialogue[d].name;
        CloseCaption(cc);

        StartCoroutine(Playing(clipLength));
        }
        else if(isCurrentlyPlaying)
        {
            if(isRunning == false)
            {
                StartCoroutine(Playing(clipLength));
            }
        }
    }

    public void DialogueCleaning()
    {
        if (isCurrentlyPlaying == false && isRunning ==false)
        {
     
        int c = Random.Range(0, dialogueManager.cleaningDialogue.Length);
        AudioSource audio = GetComponent<AudioSource>();

        audio.clip = dialogueManager.cleaningDialogue[c].clip;
        audio.Play();
        isCurrentlyPlaying = true;

        clipLength = audio.clip.length;
        Debug.Log(clipLength);

        cc = dialogueManager.cleaningDialogue[c].name;
        CloseCaption(cc);

        StartCoroutine(Playing(clipLength));
        }
        else if(isCurrentlyPlaying)
        {
            if(isRunning == false)
            {
                StartCoroutine(Playing(clipLength));
            }
        }
    }
    public void DialogueRandom()
    {
     
    int doesPlay = Random.Range(0,4);//0,1,2,3
    if(doesPlay ==0)
    {
        int r = Random.Range(0, dialogueManager.randomDialogue.Length);
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = dialogueManager.randomDialogue[r].clip;
        audio.Play();

        clipLength = audio.clip.length;
        Debug.Log(clipLength);
     
        cc = dialogueManager.randomDialogue[r].name;
        CloseCaption(cc);
        }
    }


    public void ExpositionOne()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.clip = dialogueManager.expositionDialogue[0].clip;
        audio.Play();
    }
    public void ExpositionTwo()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.clip = dialogueManager.expositionDialogue[1].clip;
        audio.Play();
    }
    public void ExpositionThree()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.clip = dialogueManager.expositionDialogue[2].clip;
        audio.Play();
    }
    public void ExpositionFour()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.clip = dialogueManager.expositionDialogue[3].clip;
        audio.Play();
    }
    public void ExpositionFive()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.clip = dialogueManager.expositionDialogue[4].clip;
        audio.Play();
    }
    public void ExpositionSix()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.clip = dialogueManager.expositionDialogue[5].clip;
        audio.Play();
    }

    public void CloseCaption(string cc)
    {
        closedCap.text = cc;
    }
     
}

