using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToWall : MonoBehaviour
{
	[SerializeField]
	private LayerMask wallLayer;

    void Start()
    {
		if(Physics.Raycast(transform.position, transform.forward, out var rayHit, 5.0f, wallLayer)) {
			transform.SetParent(rayHit.collider.transform, true);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
