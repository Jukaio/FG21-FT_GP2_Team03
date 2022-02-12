using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public DialogueInstance[] playerActions;
    public DialogueInstance[] swordSwing;
    public DialogueInstance[] environmentSounds;
    public DialogueInstance[] deathSounds;
   
   
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [System.Serializable]
    public class AudioClips
    {
    public string name;
    public AudioClip clip;
    }
}
