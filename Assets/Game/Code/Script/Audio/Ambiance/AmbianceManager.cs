using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceManager : MonoBehaviour
{
    public DialogueInstance[] ambiance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [System.Serializable]
    public class AmbianceInstance
    {
    [TextArea]
    public string name;

    public AudioClip clip;
    }
}
