using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningFeedback : MonoBehaviour
{
    [SerializeField] private ParticleSystem swordParticles;
    
    [SerializeField] private VoidEvent onClean;
    private bool isCleaning;
    // Start is called before the first frame update
    
    void OnEnable()
    {
        onClean.onEvent += SwordCleaned;
    }
    void OnDisable()
    {
        onClean.onEvent -= SwordCleaned;
        onClean.onEvent -= TurnOffParticles;
    }
    public void SwordCleaned()
    {
        swordParticles.Play();
    }
    public void TurnOffParticles()
    {
        swordParticles.Stop();
    }
    void Start()
    {
        swordParticles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
