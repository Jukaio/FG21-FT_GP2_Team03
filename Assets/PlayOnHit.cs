using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnHit : MonoBehaviour
{
    public ParticleSystem swordTrail1;
    public ParticleSystem swordTrail2;
    public ParticleSystem bloodSplatter;
    
    [SerializeField] private VoidEvent onPlayerAttack;
    // Start is called before the first frame update
    
    private void OnEnable()
	{
		onPlayerAttack.onEvent += SwordTrail;
	}
    private void OnDisable()
    {
        onPlayerAttack.onEvent -= swordTrail1.Stop;
        onPlayerAttack.onEvent -= swordTrail2.Stop;
    }
    public void SwordTrail()
    {
        swordTrail1.Play();
        swordTrail2.Play();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
