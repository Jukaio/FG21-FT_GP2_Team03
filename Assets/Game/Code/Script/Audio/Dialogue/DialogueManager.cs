using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueInstance[] attackDialogue;
    public DialogueInstance[] dirtyDialogue;
    public DialogueInstance[] cleaningDialogue;
    public DialogueInstance[] randomDialogue;
    public DialogueInstance[] expositionDialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class DialogueInstance
{
    [TextArea]
    public string name;

    public AudioClip clip;
}
